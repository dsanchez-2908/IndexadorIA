using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Data Access Layer para preparación de imágenes
    /// </summary>
    public class PreparacionImagenesDAL
    {
        /// <summary>
        /// Obtiene los lotes disponibles para preparación de imágenes
        /// </summary>
        public List<LoteGridDto> ObtenerLotesParaPreparacion(string? filtroNombre = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            var lotes = new List<LoteGridDto>();

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var query = @"
                    SELECT 
                        l.cdLote,
                        l.dsNombreLote,
                        l.feAltaLote AS FeAlta,
                        l.nuCantidadArchivos,
                        e.dsEstado AS DsEstado
                    FROM TD_LOTE l
                    INNER JOIN TD_ESTADOS e ON e.dsProceso = 'LOTE' AND e.cdEstado = l.cdEstadoLote
                    WHERE l.cdEstadoLote = 1"; // Estado: Pendiente de Preparar imágenes

                // Agregar filtros opcionales
                if (!string.IsNullOrWhiteSpace(filtroNombre))
                {
                    query += " AND l.dsNombreLote LIKE @filtroNombre";
                }

                if (fechaDesde.HasValue)
                {
                    query += " AND l.feAltaLote >= @fechaDesde";
                }

                if (fechaHasta.HasValue)
                {
                    query += " AND l.feAltaLote <= @fechaHasta";
                }

                query += " ORDER BY l.feAltaLote DESC";

                using (var comando = new SqlCommand(query, conexion))
                {
                    if (!string.IsNullOrWhiteSpace(filtroNombre))
                    {
                        comando.Parameters.AddWithValue("@filtroNombre", "%" + filtroNombre + "%");
                    }

                    if (fechaDesde.HasValue)
                    {
                        comando.Parameters.AddWithValue("@fechaDesde", fechaDesde.Value);
                    }

                    if (fechaHasta.HasValue)
                    {
                        comando.Parameters.AddWithValue("@fechaHasta", fechaHasta.Value.AddDays(1).AddSeconds(-1));
                    }

                    conexion.Open();

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lotes.Add(new LoteGridDto
                            {
                                CdLote = reader.GetInt32(0),
                                DsNombreLote = reader.GetString(1),
                                FeAlta = reader.GetDateTime(2),
                                NuCantidadArchivos = reader.GetInt32(3),
                                DsEstado = reader.GetString(4),
                                Seleccionado = false
                            });
                        }
                    }
                }
            }

            return lotes;
        }

        /// <summary>
        /// Obtiene los archivos de página asociados a un lote
        /// </summary>
        public List<ArchivoPagina> ObtenerArchivosDeLote(int cdLote)
        {
            var archivos = new List<ArchivoPagina>();

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var query = @"
                    SELECT 
                        ap.cdArchivoPagina,
                        ap.dsNombreArchivoPagina,
                        ap.dsRutaCompleta,
                        ap.nuPagina,
                        ap.cdEstado,
                        ap.feAlta,
                        ap.cdArchivoOriginal
                    FROM TD_LOTE_ARCHIVOS la
                    INNER JOIN TD_ARCHIVOS_PAGINAS ap ON ap.cdArchivoPagina = la.cdArchivoPagina
                    WHERE la.cdLote = @cdLote
                    ORDER BY ap.nuPagina";

                using (var comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdLote", cdLote);

                    conexion.Open();

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            archivos.Add(new ArchivoPagina
                            {
                                CdArchivoPagina = reader.GetInt32(0),
                                DsNombreArchivoPagina = reader.GetString(1),
                                DsRutaCompleta = reader.GetString(2),
                                NuPagina = reader.GetInt32(3),
                                CdEstado = reader.GetInt32(4),
                                FeAlta = reader.GetDateTime(5),
                                CdArchivoOriginal = reader.GetInt32(6)
                            });
                        }
                    }
                }
            }

            return archivos;
        }

        /// <summary>
        /// Actualiza el estado de un lote
        /// </summary>
        public void ActualizarEstadoLote(int cdLote, int nuevoEstado)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var query = "UPDATE TD_LOTE SET cdEstadoLote = @nuevoEstado WHERE cdLote = @cdLote";

                using (var comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdLote", cdLote);
                    comando.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Actualiza el estado de un archivo de página
        /// </summary>
        public void ActualizarEstadoArchivoPagina(int cdArchivoPagina, int nuevoEstado)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var query = "UPDATE TD_ARCHIVOS_PAGINAS SET cdEstado = @nuevoEstado WHERE cdArchivoPagina = @cdArchivoPagina";

                using (var comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);
                    comando.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Inserta el resultado de OCR de una página
        /// </summary>
        public void InsertarResultadoOCR(int cdArchivoPagina, string textoOCR)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var query = @"
                    INSERT INTO TD_PAGINA_OCR (cdArchivoPagina, txtResultadoOCR, feOCR)
                    VALUES (@cdArchivoPagina, @textoOCR, GETDATE())";

                using (var comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);
                    comando.Parameters.Add("@textoOCR", SqlDbType.NVarChar, -1).Value = textoOCR ?? (object)DBNull.Value;

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
