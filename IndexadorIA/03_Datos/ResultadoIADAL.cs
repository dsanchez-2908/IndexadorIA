using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_001_RESULTADO_IA
    /// </summary>
    public class ResultadoIADAL
    {
        private readonly string _cadenaConexion;

        public ResultadoIADAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Inserta un nuevo resultado de IA y retorna el ID generado
        /// </summary>
        public int Insertar(ResultadoIA resultado)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    INSERT INTO TD_001_RESULTADO_IA 
                    (cdLote, cdArchivoPagina, cdTipoPlano, dsExpediente, dsSeccion, dsManzana, dsParcela, dsDireccion,
                     nuConfianzaTipoPlano, nuConfianzaExpediente, nuConfianzaSeccion, nuConfianzaManzana, 
                     nuConfianzaParcela, nuConfianzaDireccion, feAlta, cdUsuarioAlta)
                    VALUES 
                    (@cdLote, @cdArchivoPagina, @cdTipoPlano, @dsExpediente, @dsSeccion, @dsManzana, @dsParcela, @dsDireccion,
                     @nuConfianzaTipoPlano, @nuConfianzaExpediente, @nuConfianzaSeccion, @nuConfianzaManzana, 
                     @nuConfianzaParcela, @nuConfianzaDireccion, GETDATE(), @cdUsuarioAlta);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);", conn);

                cmd.Parameters.AddWithValue("@cdLote", resultado.CdLote);
                cmd.Parameters.AddWithValue("@cdArchivoPagina", resultado.CdArchivoPagina);
                cmd.Parameters.AddWithValue("@cdTipoPlano", (object?)resultado.CdTipoPlano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsExpediente", (object?)resultado.DsExpediente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsSeccion", (object?)resultado.DsSeccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsManzana", (object?)resultado.DsManzana ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsParcela", (object?)resultado.DsParcela ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsDireccion", (object?)resultado.DsDireccion ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@nuConfianzaTipoPlano", (object?)resultado.NuConfianzaTipoPlano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaExpediente", (object?)resultado.NuConfianzaExpediente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaSeccion", (object?)resultado.NuConfianzaSeccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaManzana", (object?)resultado.NuConfianzaManzana ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaParcela", (object?)resultado.NuConfianzaParcela ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaDireccion", (object?)resultado.NuConfianzaDireccion ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@cdUsuarioAlta", resultado.CdUsuarioAlta);

                conn.Open();
                int cdResultado = (int)cmd.ExecuteScalar()!;

                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "ResultadoIADAL.Insertar",
                    DsMensaje = $"Resultado IA insertado: {cdResultado} para archivo/página {resultado.CdArchivoPagina}"
                });

                return cdResultado;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "ResultadoIADAL.Insertar",
                    DsMensaje = $"Error al insertar resultado IA: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los resultados de un lote
        /// </summary>
        public List<ResultadoIA> ObtenerPorLote(int cdLote)
        {
            List<ResultadoIA> resultados = new();

            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    SELECT r.cdResultado, r.cdLote, r.cdArchivoPagina, r.cdTipoPlano, 
                           r.dsExpediente, r.dsSeccion, r.dsManzana, r.dsParcela, r.dsDireccion,
                           r.nuConfianzaTipoPlano, r.nuConfianzaExpediente, r.nuConfianzaSeccion, 
                           r.nuConfianzaManzana, r.nuConfianzaParcela, r.nuConfianzaDireccion,
                           r.feAlta, r.cdUsuarioAlta, r.feUltimaModificacion, r.cdUsuarioModificacion,
                           tp.dsTipoPlano, a.dsNombreArchivo, ap.nuPagina
                    FROM TD_001_RESULTADO_IA r
                    LEFT JOIN TD_TIPOS_PLANO tp ON r.cdTipoPlano = tp.cdTipoPlano
                    LEFT JOIN TD_ARCHIVOS_PAGINAS ap ON r.cdArchivoPagina = ap.cdArchivoPagina
                    LEFT JOIN TD_ARCHIVOS a ON ap.cdArchivo = a.cdArchivo
                    WHERE r.cdLote = @cdLote
                    ORDER BY a.dsNombreArchivo, ap.nuPagina", conn);

                cmd.Parameters.AddWithValue("@cdLote", cdLote);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    resultados.Add(new ResultadoIA
                    {
                        CdResultado = reader.GetInt32(0),
                        CdLote = reader.GetInt32(1),
                        CdArchivoPagina = reader.GetInt32(2),
                        CdTipoPlano = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        DsExpediente = reader.IsDBNull(4) ? null : reader.GetString(4),
                        DsSeccion = reader.IsDBNull(5) ? null : reader.GetString(5),
                        DsManzana = reader.IsDBNull(6) ? null : reader.GetString(6),
                        DsParcela = reader.IsDBNull(7) ? null : reader.GetString(7),
                        DsDireccion = reader.IsDBNull(8) ? null : reader.GetString(8),
                        NuConfianzaTipoPlano = reader.IsDBNull(9) ? null : reader.GetDecimal(9),
                        NuConfianzaExpediente = reader.IsDBNull(10) ? null : reader.GetDecimal(10),
                        NuConfianzaSeccion = reader.IsDBNull(11) ? null : reader.GetDecimal(11),
                        NuConfianzaManzana = reader.IsDBNull(12) ? null : reader.GetDecimal(12),
                        NuConfianzaParcela = reader.IsDBNull(13) ? null : reader.GetDecimal(13),
                        NuConfianzaDireccion = reader.IsDBNull(14) ? null : reader.GetDecimal(14),
                        FeAlta = reader.GetDateTime(15),
                        CdUsuarioAlta = reader.GetInt32(16),
                        FeUltimaModificacion = reader.IsDBNull(17) ? null : reader.GetDateTime(17),
                        CdUsuarioModificacion = reader.IsDBNull(18) ? null : reader.GetInt32(18),
                        DsTipoPlano = reader.IsDBNull(19) ? null : reader.GetString(19),
                        NombreArchivo = reader.IsDBNull(20) ? null : reader.GetString(20),
                        NumeroPagina = reader.IsDBNull(21) ? null : reader.GetInt32(21)
                    });
                }

                return resultados;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "ResultadoIADAL.ObtenerPorLote",
                    DsMensaje = $"Error al obtener resultados del lote {cdLote}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
    }
}
