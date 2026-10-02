using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la consulta de planos, basada en la vista VW_PLANOS_CONSULTAS.
    /// </summary>
    public class PlanoConsultaDAL
    {
        private readonly string _cadenaConexion;

        public PlanoConsultaDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Consulta la vista VW_PLANOS_CONSULTAS aplicando los filtros indicados.
        /// </summary>
        public List<PlanoConsulta> Consultar(PlanoConsultaFiltro filtro)
        {
            var resultado = new List<PlanoConsulta>();

            try
            {
                var condiciones = new List<string>();
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand { Connection = conn };

                if (filtro.CdLote.HasValue)
                {
                    condiciones.Add("cdLote = @cdLote");
                    cmd.Parameters.AddWithValue("@cdLote", filtro.CdLote.Value);
                }

                if (filtro.CdEstadoLote.HasValue)
                {
                    condiciones.Add("cdEstadoLote = @cdEstadoLote");
                    cmd.Parameters.AddWithValue("@cdEstadoLote", filtro.CdEstadoLote.Value);
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsDescripcionLote))
                {
                    condiciones.Add("dsDescripcion LIKE @dsDescripcionLote");
                    cmd.Parameters.AddWithValue("@dsDescripcionLote", $"%{filtro.DsDescripcionLote}%");
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsEntrega))
                {
                    condiciones.Add("dsEntrega LIKE @dsEntrega");
                    cmd.Parameters.AddWithValue("@dsEntrega", $"%{filtro.DsEntrega}%");
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsCarpeta))
                {
                    condiciones.Add("dsCarpeta LIKE @dsCarpeta");
                    cmd.Parameters.AddWithValue("@dsCarpeta", $"%{filtro.DsCarpeta}%");
                }

                if (filtro.CdEstadoControl.HasValue)
                {
                    condiciones.Add("cdEstadoControl = @cdEstadoControl");
                    cmd.Parameters.AddWithValue("@cdEstadoControl", filtro.CdEstadoControl.Value);
                }

                if (filtro.CdCategoriaPlano.HasValue)
                {
                    condiciones.Add("cdCategoriaPlano = @cdCategoriaPlano");
                    cmd.Parameters.AddWithValue("@cdCategoriaPlano", filtro.CdCategoriaPlano.Value);
                }

                if (filtro.CdTipoPlano.HasValue)
                {
                    condiciones.Add("cdTipoPlano = @cdTipoPlano");
                    cmd.Parameters.AddWithValue("@cdTipoPlano", filtro.CdTipoPlano.Value);
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsExpediente))
                {
                    condiciones.Add("dsExpediente LIKE @dsExpediente");
                    cmd.Parameters.AddWithValue("@dsExpediente", $"%{filtro.DsExpediente}%");
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsSeccion))
                {
                    condiciones.Add("dsSeccion LIKE @dsSeccion");
                    cmd.Parameters.AddWithValue("@dsSeccion", $"%{filtro.DsSeccion}%");
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsManzana))
                {
                    condiciones.Add("dsManzana LIKE @dsManzana");
                    cmd.Parameters.AddWithValue("@dsManzana", $"%{filtro.DsManzana}%");
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsParcela))
                {
                    condiciones.Add("dsParcela LIKE @dsParcela");
                    cmd.Parameters.AddWithValue("@dsParcela", $"%{filtro.DsParcela}%");
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsDireccion))
                {
                    condiciones.Add("dsDireccion LIKE @dsDireccion");
                    cmd.Parameters.AddWithValue("@dsDireccion", $"%{filtro.DsDireccion}%");
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsNumeroPlano))
                {
                    condiciones.Add("dsNumeroPlano LIKE @dsNumeroPlano");
                    cmd.Parameters.AddWithValue("@dsNumeroPlano", $"%{filtro.DsNumeroPlano}%");
                }

                if (!string.IsNullOrWhiteSpace(filtro.DsObservaciones))
                {
                    condiciones.Add("dsObservaciones LIKE @dsObservaciones");
                    cmd.Parameters.AddWithValue("@dsObservaciones", $"%{filtro.DsObservaciones}%");
                }

                var sql = @"
                    SELECT TOP (@topRegistros)
                           cdResultado, cdLote, cdEstadoLote, dsEstadoLote, dsDescripcion, dsEntrega, dsCarpeta,
                           cdEstadoControl, dsEstadoControl, cdArchivoPagina, cdCategoriaPlano, dsCategoriaPlano,
                           cdTipoPlano, dsTipoPlano, dsExpediente, dsSeccion, dsManzana, dsParcela, dsDireccion,
                           dsNumeroPlano, dsObservaciones, dsNombreArchivoOriginal, dsNombreArchivoPagina, nuPagina
                    FROM VW_PLANOS_CONSULTAS";

                cmd.Parameters.AddWithValue("@topRegistros", filtro.MaxRegistros);

                if (condiciones.Count > 0)
                {
                    sql += " WHERE " + string.Join(" AND ", condiciones);
                }

                sql += " ORDER BY cdLote, cdResultado";

                cmd.CommandText = sql;

                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    resultado.Add(new PlanoConsulta
                    {
                        CdResultado = reader.GetInt32(reader.GetOrdinal("cdResultado")),
                        CdLote = reader.GetInt32(reader.GetOrdinal("cdLote")),
                        CdEstadoLote = reader.IsDBNull(reader.GetOrdinal("cdEstadoLote")) ? null : reader.GetInt32(reader.GetOrdinal("cdEstadoLote")),
                        DsEstadoLote = reader.IsDBNull(reader.GetOrdinal("dsEstadoLote")) ? null : reader.GetString(reader.GetOrdinal("dsEstadoLote")),
                        DsDescripcionLote = reader.IsDBNull(reader.GetOrdinal("dsDescripcion")) ? null : reader.GetString(reader.GetOrdinal("dsDescripcion")),
                        DsEntrega = reader.IsDBNull(reader.GetOrdinal("dsEntrega")) ? null : reader.GetString(reader.GetOrdinal("dsEntrega")),
                        DsCarpeta = reader.IsDBNull(reader.GetOrdinal("dsCarpeta")) ? null : reader.GetString(reader.GetOrdinal("dsCarpeta")),
                        CdEstadoControl = reader.IsDBNull(reader.GetOrdinal("cdEstadoControl")) ? null : reader.GetInt32(reader.GetOrdinal("cdEstadoControl")),
                        DsEstadoControl = reader.IsDBNull(reader.GetOrdinal("dsEstadoControl")) ? null : reader.GetString(reader.GetOrdinal("dsEstadoControl")),
                        CdArchivoPagina = reader.IsDBNull(reader.GetOrdinal("cdArchivoPagina")) ? null : reader.GetInt32(reader.GetOrdinal("cdArchivoPagina")),
                        CdCategoriaPlano = reader.IsDBNull(reader.GetOrdinal("cdCategoriaPlano")) ? null : reader.GetInt32(reader.GetOrdinal("cdCategoriaPlano")),
                        DsCategoriaPlano = reader.IsDBNull(reader.GetOrdinal("dsCategoriaPlano")) ? null : reader.GetString(reader.GetOrdinal("dsCategoriaPlano")),
                        CdTipoPlano = reader.IsDBNull(reader.GetOrdinal("cdTipoPlano")) ? null : reader.GetInt32(reader.GetOrdinal("cdTipoPlano")),
                        DsTipoPlano = reader.IsDBNull(reader.GetOrdinal("dsTipoPlano")) ? null : reader.GetString(reader.GetOrdinal("dsTipoPlano")),
                        DsExpediente = reader.IsDBNull(reader.GetOrdinal("dsExpediente")) ? null : reader.GetString(reader.GetOrdinal("dsExpediente")),
                        DsSeccion = reader.IsDBNull(reader.GetOrdinal("dsSeccion")) ? null : reader.GetString(reader.GetOrdinal("dsSeccion")),
                        DsManzana = reader.IsDBNull(reader.GetOrdinal("dsManzana")) ? null : reader.GetString(reader.GetOrdinal("dsManzana")),
                        DsParcela = reader.IsDBNull(reader.GetOrdinal("dsParcela")) ? null : reader.GetString(reader.GetOrdinal("dsParcela")),
                        DsDireccion = reader.IsDBNull(reader.GetOrdinal("dsDireccion")) ? null : reader.GetString(reader.GetOrdinal("dsDireccion")),
                        DsNumeroPlano = reader.IsDBNull(reader.GetOrdinal("dsNumeroPlano")) ? null : reader.GetString(reader.GetOrdinal("dsNumeroPlano")),
                        DsObservaciones = reader.IsDBNull(reader.GetOrdinal("dsObservaciones")) ? null : reader.GetString(reader.GetOrdinal("dsObservaciones")),
                        DsNombreArchivoOriginal = reader.IsDBNull(reader.GetOrdinal("dsNombreArchivoOriginal")) ? null : reader.GetString(reader.GetOrdinal("dsNombreArchivoOriginal")),
                        DsNombreArchivoPagina = reader.IsDBNull(reader.GetOrdinal("dsNombreArchivoPagina")) ? null : reader.GetString(reader.GetOrdinal("dsNombreArchivoPagina")),
                        NuPagina = reader.IsDBNull(reader.GetOrdinal("nuPagina")) ? null : reader.GetInt32(reader.GetOrdinal("nuPagina"))
                    });
                }

                return resultado;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PlanoConsultaDAL.Consultar",
                    DsMensaje = $"Error al consultar planos: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Obtiene los estados de lote (dsProceso='LOTE') utilizados como filtro.
        /// </summary>
        public List<KeyValuePair<int, string>> ObtenerEstadosLoteParaFiltro()
        {
            var estados = new List<KeyValuePair<int, string>>();

            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(
                    "SELECT cdEstado, dsEstado FROM TD_ESTADOS WHERE dsProceso='LOTE' ORDER BY cdEstado", conn);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    estados.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                }

                return estados;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PlanoConsultaDAL.ObtenerEstadosLoteParaFiltro",
                    DsMensaje = $"Error al obtener estados de lote: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Obtiene los estados de control (dsProceso='CONTROL') utilizados como filtro.
        /// </summary>
        public List<KeyValuePair<int, string>> ObtenerEstadosControlParaFiltro()
        {
            var estados = new List<KeyValuePair<int, string>>();

            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(
                    "SELECT cdEstado, dsEstado FROM TD_ESTADOS WHERE dsProceso='CONTROL' ORDER BY cdEstado", conn);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    estados.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                }

                return estados;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PlanoConsultaDAL.ObtenerEstadosControlParaFiltro",
                    DsMensaje = $"Error al obtener estados de control: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
    }
}
