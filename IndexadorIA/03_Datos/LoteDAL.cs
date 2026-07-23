using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Clase de acceso a datos para lotes de archivos página
    /// </summary>
    public class LoteDAL
    {
        private readonly string _cadenaConexion;

        public LoteDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene el siguiente número de secuencia para nombre de lote
        /// </summary>
        public int ObtenerSiguienteSecuencia()
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand("SELECT NEXT VALUE FOR SEQ_LOTE", conexion);
                conexion.Open();
                var resultado = comando.ExecuteScalar();
                return Convert.ToInt32(resultado);
            }
        }

        /// <summary>
        /// Crea un nuevo lote con sus archivos página en una transacción
        /// </summary>
        public int CrearLote(string dsNombreLote, int nuCantidadArchivos, int cdEstadoLote, 
                            int cdUsuarioAltaLote, List<int> cdArchivosPagina)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar lote
                        var cmdLote = new SqlCommand(
                            @"INSERT INTO TD_LOTE (dsNombreLote, nuCantidadArchivos, cdEstadoLote, 
                                                   feAltaLote, cdUsuarioAltaLote)
                              OUTPUT INSERTED.cdLote
                              VALUES (@dsNombreLote, @nuCantidadArchivos, @cdEstadoLote, 
                                     GETDATE(), @cdUsuarioAltaLote)", conexion, transaccion);

                        cmdLote.Parameters.AddWithValue("@dsNombreLote", dsNombreLote);
                        cmdLote.Parameters.AddWithValue("@nuCantidadArchivos", nuCantidadArchivos);
                        cmdLote.Parameters.AddWithValue("@cdEstadoLote", cdEstadoLote);
                        cmdLote.Parameters.AddWithValue("@cdUsuarioAltaLote", cdUsuarioAltaLote);

                        var resultadoLote = cmdLote.ExecuteScalar();
                        int cdLote = Convert.ToInt32(resultadoLote);

                        // 2. Insertar relaciones lote-archivos
                        foreach (int cdArchivoPagina in cdArchivosPagina)
                        {
                            var cmdRelacion = new SqlCommand(
                                @"INSERT INTO TD_LOTE_ARCHIVOS (cdLote, cdArchivoPagina)
                                  VALUES (@cdLote, @cdArchivoPagina)", conexion, transaccion);

                            cmdRelacion.Parameters.AddWithValue("@cdLote", cdLote);
                            cmdRelacion.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);
                            cmdRelacion.ExecuteNonQuery();
                        }

                        // 3. Actualizar estado de las páginas a cdEstado=2
                        foreach (int cdArchivoPagina in cdArchivosPagina)
                        {
                            var cmdUpdate = new SqlCommand(
                                @"UPDATE TD_ARCHIVOS_PAGINAS 
                                  SET cdEstado = 2 
                                  WHERE cdArchivoPagina = @cdArchivoPagina", conexion, transaccion);

                            cmdUpdate.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                        return cdLote;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene archivos página filtrados para preparación de lotes
        /// </summary>
        public List<ArchivoPaginaGridDto> ObtenerArchivosPaginaParaLote(int cdProyecto, int cdEstado,
            string? dsNombreArchivo = null, DateTime? feAltaDesde = null, DateTime? feAltaHasta = null)
        {
            var archivos = new List<ArchivoPaginaGridDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var query = @"
                    SELECT a.cdArchivoPagina,
                           a.feAlta,
                           a.cdEstado,
                           c.dsEstado,
                           a.dsNombreArchivoPagina,
                           a.nuPagina,
                           a.dsRutaCompleta,
                           a.cdArchivoOriginal,
                           b.cdProyecto,
                           d.dsProyecto,
                           b.dsNombreArchivo,
                           b.dsNombreUltimaCarpeta,
                           b.nuCantidadPaginas			
                    FROM TD_ARCHIVOS_PAGINAS a
                    LEFT JOIN TD_ARCHIVOS_ORIGINAL b ON a.cdArchivoOriginal = b.cdArchivo
                    LEFT JOIN TD_ESTADOS c ON c.dsProceso = 'ARCHIVO_PAGINA' AND a.cdEstado = c.cdEstado
                    LEFT JOIN TD_PROYECTOS d ON b.cdProyecto = d.cdProyecto
                    WHERE b.cdProyecto = @cdProyecto AND a.cdEstado = @cdEstado";

                var comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);
                comando.Parameters.AddWithValue("@cdEstado", cdEstado);

                // Filtros opcionales
                if (!string.IsNullOrEmpty(dsNombreArchivo))
                {
                    comando.CommandText += " AND b.dsNombreArchivo LIKE @dsNombreArchivo";
                    comando.Parameters.AddWithValue("@dsNombreArchivo", $"%{dsNombreArchivo}%");
                }

                if (feAltaDesde.HasValue)
                {
                    comando.CommandText += " AND a.feAlta >= @feAltaDesde";
                    comando.Parameters.AddWithValue("@feAltaDesde", feAltaDesde.Value);
                }

                if (feAltaHasta.HasValue)
                {
                    comando.CommandText += " AND a.feAlta <= @feAltaHasta";
                    comando.Parameters.AddWithValue("@feAltaHasta", feAltaHasta.Value.AddDays(1).AddSeconds(-1));
                }

                comando.CommandText += " ORDER BY a.feAlta, a.cdArchivoPagina";

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        archivos.Add(new ArchivoPaginaGridDto
                        {
                            CdArchivoPagina = lector.GetInt32(0),
                            FeAlta = lector.GetDateTime(1),
                            CdEstado = lector.GetInt32(2),
                            DsEstado = lector.IsDBNull(3) ? "" : lector.GetString(3),
                            DsNombreArchivoPagina = lector.GetString(4),
                            NuPagina = lector.GetInt32(5),
                            DsRutaCompleta = lector.GetString(6),
                            CdArchivoOriginal = lector.GetInt32(7),
                            CdProyecto = lector.IsDBNull(8) ? 0 : lector.GetInt32(8),
                            DsProyecto = lector.IsDBNull(9) ? "" : lector.GetString(9),
                            DsNombreArchivo = lector.IsDBNull(10) ? "" : lector.GetString(10),
                            DsNombreUltimaCarpeta = lector.IsDBNull(11) ? "" : lector.GetString(11),
                            NuCantidadPaginas = lector.IsDBNull(12) ? 0 : lector.GetInt32(12)
                        });
                    }
                }
            }

            return archivos;
        }

        /// <summary>
        /// Actualiza el estado de un lote
        /// </summary>
        public void ActualizarEstado(int cdLote, int cdEstado, int cdUsuario)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    UPDATE TD_LOTE 
                    SET cdEstadoLote = @cdEstado
                    WHERE cdLote = @cdLote", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);
                comando.Parameters.AddWithValue("@cdEstado", cdEstado);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Obtiene los archivos/páginas de un lote con sus rutas Base64
        /// </summary>
        public List<ArchivoPagina> ObtenerArchivosPaginasPorLote(int cdLote)
        {
            var archivos = new List<ArchivoPagina>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    SELECT ap.cdArchivoPagina, ap.dsNombreArchivoPagina, ap.nuPagina, 
                           ap.dsRutaCompleta, a.dsNombreArchivo,
                           REPLACE(REPLACE(ap.dsRutaCompleta, '.png', '.b64'), '.pdf', '.b64') as RutaBase64
                    FROM TD_LOTE_ARCHIVOS la
                    INNER JOIN TD_ARCHIVOS_PAGINAS ap ON la.cdArchivoPagina = ap.cdArchivoPagina
                    INNER JOIN TD_ARCHIVOS_ORIGINAL a ON ap.cdArchivoOriginal = a.cdArchivo
                    WHERE la.cdLote = @cdLote
                    ORDER BY a.dsNombreArchivo, ap.nuPagina", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        archivos.Add(new ArchivoPagina
                        {
                            CdArchivoPagina = lector.GetInt32(0),
                            DsNombreArchivoPagina = lector.GetString(1),
                            NuPagina = lector.GetInt32(2),
                            DsRutaCompleta = lector.GetString(3),
                            NombreArchivo = lector.GetString(4),
                            RutaBase64 = lector.GetString(5)
                        });
                    }
                }
            }

            return archivos;
        }

        /// <summary>
        /// Obtiene la fecha de finalización del procesamiento IA (batch) de un lote,
        /// o null si el lote aún no fue procesado / no tiene batch completado.
        /// </summary>
        public DateTime? ObtenerFechaProcesamientoIA(int cdLote)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    SELECT MAX(feCompleted)
                    FROM TD_BATCH_TRACKING
                    WHERE cdLote = @cdLote", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);

                conexion.Open();
                var resultado = comando.ExecuteScalar();

                return (resultado == null || resultado == DBNull.Value)
                    ? (DateTime?)null
                    : Convert.ToDateTime(resultado);
            }
        }

        /// <summary>
        /// Obtiene un lote por su identificador.
        /// </summary>
        public Lote? ObtenerPorId(int cdLote)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    SELECT l.cdLote, l.dsNombreLote, l.nuCantidadArchivos, l.cdEstadoLote,
                           l.feAltaLote, l.cdUsuarioAltaLote, e.dsEstado
                    FROM TD_LOTE l
                    LEFT JOIN TD_ESTADOS e ON e.dsProceso = 'LOTE' AND e.cdEstado = l.cdEstadoLote
                    WHERE l.cdLote = @cdLote", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return new Lote
                        {
                            CdLote = lector.GetInt32(0),
                            DsNombreLote = lector.GetString(1),
                            NuCantidadArchivos = lector.GetInt32(2),
                            CdEstadoLote = lector.GetInt32(3),
                            FeAltaLote = lector.GetDateTime(4),
                            CdUsuarioAltaLote = lector.IsDBNull(5) ? null : lector.GetInt32(5),
                            DsEstado = lector.IsDBNull(6) ? null : lector.GetString(6)
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Obtiene los lotes con un estado específico para procesamiento
        /// </summary>
        public List<Lote> ObtenerLotesPorEstado(int cdEstado, int? cdProyecto = null)
        {
            var lotes = new List<Lote>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var sql = @"
                    SELECT l.cdLote, l.dsNombreLote, l.nuCantidadArchivos, 
                           l.cdEstadoLote, e.dsEstado, l.feAltaLote
                    FROM TD_LOTE l
                    INNER JOIN TD_ESTADOS e ON e.dsProceso = 'LOTE' AND e.cdEstado = l.cdEstadoLote
                    WHERE l.cdEstadoLote = @cdEstado";

                if (cdProyecto.HasValue)
                {
                    sql += " AND EXISTS (SELECT 1 FROM TD_LOTE_ARCHIVOS la " +
                           "INNER JOIN TD_ARCHIVOS_PAGINAS ap ON la.cdArchivoPagina = ap.cdArchivoPagina " +
                           "INNER JOIN TD_ARCHIVOS_ORIGINAL a ON ap.cdArchivoOriginal = a.cdArchivo " +
                           "WHERE la.cdLote = l.cdLote AND a.cdProyecto = @cdProyecto)";
                }

                sql += " ORDER BY l.feAltaLote DESC";

                var comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@cdEstado", cdEstado);
                if (cdProyecto.HasValue)
                    comando.Parameters.AddWithValue("@cdProyecto", cdProyecto.Value);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lotes.Add(new Lote
                        {
                            CdLote = lector.GetInt32(0),
                            DsNombreLote = lector.GetString(1),
                            NuCantidadArchivos = lector.GetInt32(2),
                            CdEstadoLote = lector.GetInt32(3),
                            DsEstado = lector.GetString(4),
                            FeAltaLote = lector.GetDateTime(5)
                        });
                    }
                }
            }

            return lotes;
        }

        /// <summary>
        /// Obtiene lotes filtrados por estado, nombre de lote y rango de fecha de alta
        /// </summary>
        public List<Lote> ObtenerLotesPorEstadoFiltrado(int cdEstado, string? dsNombreLote = null,
            DateTime? feAltaDesde = null, DateTime? feAltaHasta = null)
        {
            var lotes = new List<Lote>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var sql = @"
                    SELECT l.cdLote, l.dsNombreLote, l.nuCantidadArchivos, 
                           l.cdEstadoLote, e.dsEstado, l.feAltaLote
                    FROM TD_LOTE l
                    INNER JOIN TD_ESTADOS e ON e.dsProceso = 'LOTE' AND e.cdEstado = l.cdEstadoLote
                    WHERE l.cdEstadoLote = @cdEstado";

                if (!string.IsNullOrEmpty(dsNombreLote))
                {
                    sql += " AND l.dsNombreLote LIKE @dsNombreLote";
                }

                if (feAltaDesde.HasValue)
                {
                    sql += " AND l.feAltaLote >= @feAltaDesde";
                }

                if (feAltaHasta.HasValue)
                {
                    sql += " AND l.feAltaLote < @feAltaHasta";
                }

                sql += " ORDER BY l.feAltaLote DESC";

                var comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@cdEstado", cdEstado);

                if (!string.IsNullOrEmpty(dsNombreLote))
                    comando.Parameters.AddWithValue("@dsNombreLote", $"%{dsNombreLote}%");

                if (feAltaDesde.HasValue)
                    comando.Parameters.AddWithValue("@feAltaDesde", feAltaDesde.Value.Date);

                if (feAltaHasta.HasValue)
                    comando.Parameters.AddWithValue("@feAltaHasta", feAltaHasta.Value.Date.AddDays(1));

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lotes.Add(new Lote
                        {
                            CdLote = lector.GetInt32(0),
                            DsNombreLote = lector.GetString(1),
                            NuCantidadArchivos = lector.GetInt32(2),
                            CdEstadoLote = lector.GetInt32(3),
                            DsEstado = lector.GetString(4),
                            FeAltaLote = lector.GetDateTime(5)
                        });
                    }
                }
            }

            return lotes;
        }
    }
}
