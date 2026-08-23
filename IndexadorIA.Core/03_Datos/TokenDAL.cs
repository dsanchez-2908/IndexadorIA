using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;
using System.Collections.Generic;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_TOKEN
    /// </summary>
    public class TokenDAL
    {
        private readonly string _cadenaConexion;

        public TokenDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Inserta un registro de consumo de tokens
        /// </summary>
        public void Insertar(TokenConsumo token)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    INSERT INTO TD_TOKEN 
                    (cdResultado, nuTokensPrompt, nuTokensCompletion, nuTokensTotal, dsModelo, feAlta)
                    VALUES 
                    (@cdResultado, @nuTokensPrompt, @nuTokensCompletion, @nuTokensTotal, @dsModelo, GETDATE())", conn);

                cmd.Parameters.AddWithValue("@cdResultado", token.CdResultado);
                cmd.Parameters.AddWithValue("@nuTokensPrompt", (object?)token.NuTokensPrompt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuTokensCompletion", (object?)token.NuTokensCompletion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuTokensTotal", (object?)token.NuTokensTotal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsModelo", (object?)token.DsModelo ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();

                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "TokenDAL.Insertar",
                    DsMensaje = $"Token registrado para resultado {token.CdResultado}: {token.NuTokensTotal} tokens totales"
                });
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "TokenDAL.Insertar",
                    DsMensaje = $"Error al insertar consumo de tokens: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Obtiene el consumo total de tokens de un lote
        /// </summary>
        public int ObtenerTotalPorLote(int cdLote)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    SELECT ISNULL(SUM(t.nuTokensTotal), 0)
                    FROM TD_TOKEN t
                    INNER JOIN TD_001_RESULTADO_IA r ON t.cdResultado = r.cdResultado
                    WHERE r.cdLote = @cdLote", conn);

                cmd.Parameters.AddWithValue("@cdLote", cdLote);

                conn.Open();
                object? result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "TokenDAL.ObtenerTotalPorLote",
                    DsMensaje = $"Error al obtener total de tokens del lote {cdLote}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
            /// <summary>
            /// Obtiene el resumen general de consumo de tokens (todos los registros de TD_TOKEN),
            /// calculando el costo en USD según los precios configurados en TD_PARAMETROS
            /// (cdParametro=6: precio por millón de tokens de ingreso; cdParametro=7: precio por millón de tokens de salida).
            /// </summary>
            public ResumenConsumoTokenDto ObtenerResumenGeneral()
            {
                try
                {
                    using var conn = new SqlConnection(_cadenaConexion);
                    using var cmd = new SqlCommand(@"
                        SELECT
                            ISNULL(SUM(t.nuTokensPrompt), 0) AS nuTotalTokensPrompt,
                            ISNULL(SUM(t.nuTokensCompletion), 0) AS nuTotalTokensCompletion,
                            ISNULL(SUM(t.nuTokensTotal), 0) AS nuTotalTokensTotal,
                            ISNULL(SUM((t.nuTokensPrompt / 1000000.0) * p.nuPrecioIngreso), 0) AS nuCostoIngresoUsd,
                            ISNULL(SUM((t.nuTokensCompletion / 1000000.0) * p.nuPrecioSalida), 0) AS nuCostoSalidaUsd,
                            COUNT(*) AS nuTotalArchivos
                        FROM TD_TOKEN t
                        CROSS JOIN (
                            SELECT
                                CAST((SELECT dsValorParametro FROM TD_PARAMETROS WHERE cdParametro = 6) AS DECIMAL(10,6)) AS nuPrecioIngreso,
                                CAST((SELECT dsValorParametro FROM TD_PARAMETROS WHERE cdParametro = 7) AS DECIMAL(10,6)) AS nuPrecioSalida
                        ) p
                        WHERE t.nuTokensTotal IS NOT NULL", conn);

                    conn.Open();
                    using var reader = cmd.ExecuteReader();

                    var resumen = new ResumenConsumoTokenDto();

                    if (reader.Read())
                    {
                        resumen.NuTotalTokensPrompt = Convert.ToInt64(reader.GetValue(0));
                        resumen.NuTotalTokensCompletion = Convert.ToInt64(reader.GetValue(1));
                        resumen.NuTotalTokensTotal = Convert.ToInt64(reader.GetValue(2));
                        resumen.NuCostoIngresoUsd = reader.GetDecimal(3);
                        resumen.NuCostoSalidaUsd = reader.GetDecimal(4);
                        resumen.NuCostoTotalUsd = resumen.NuCostoIngresoUsd + resumen.NuCostoSalidaUsd;
                        resumen.NuTotalArchivos = Convert.ToInt32(reader.GetValue(5));
                    }

                    return resumen;
                }
                catch (Exception ex)
                {
                    var logDAL = new LogDAL();
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.ERROR,
                        DsModulo = "TokenDAL.ObtenerResumenGeneral",
                        DsMensaje = $"Error al obtener el resumen general de consumo de tokens: {ex.Message}",
                        DsExcepcion = ex.ToString()
                    });
                    throw;
                }
            }

            /// <summary>
            /// Obtiene el resumen de consumo de tokens agrupado por mes/año y modelo de OpenAI utilizado.
            /// </summary>
            public List<ResumenConsumoTokenPorMesDto> ObtenerResumenPorMesYModelo()
            {
                try
                {
                    var lista = new List<ResumenConsumoTokenPorMesDto>();

                    using var conn = new SqlConnection(_cadenaConexion);
                    using var cmd = new SqlCommand(@"
                        SELECT
                            FORMAT(t.feAlta, 'yyyy-MM') AS dsPeriodo,
                            ISNULL(t.dsModelo, 'N/D') AS dsModelo,
                            ISNULL(SUM(t.nuTokensPrompt), 0) AS nuTotalTokensPrompt,
                            ISNULL(SUM(t.nuTokensCompletion), 0) AS nuTotalTokensCompletion,
                            ISNULL(SUM(t.nuTokensTotal), 0) AS nuTotalTokensTotal,
                            ISNULL(SUM((t.nuTokensPrompt / 1000000.0) * p.nuPrecioIngreso), 0) AS nuCostoIngresoUsd,
                            ISNULL(SUM((t.nuTokensCompletion / 1000000.0) * p.nuPrecioSalida), 0) AS nuCostoSalidaUsd,
                            COUNT(*) AS nuTotalArchivos
                        FROM TD_TOKEN t
                        CROSS JOIN (
                            SELECT
                                CAST((SELECT dsValorParametro FROM TD_PARAMETROS WHERE cdParametro = 6) AS DECIMAL(10,6)) AS nuPrecioIngreso,
                                CAST((SELECT dsValorParametro FROM TD_PARAMETROS WHERE cdParametro = 7) AS DECIMAL(10,6)) AS nuPrecioSalida
                        ) p
                        WHERE t.nuTokensTotal IS NOT NULL
                        GROUP BY FORMAT(t.feAlta, 'yyyy-MM'), ISNULL(t.dsModelo, 'N/D')
                        ORDER BY dsPeriodo DESC, dsModelo", conn);

                    conn.Open();
                    using var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var costoIngreso = reader.GetDecimal(5);
                        var costoSalida = reader.GetDecimal(6);

                        lista.Add(new ResumenConsumoTokenPorMesDto
                        {
                            DsPeriodo = reader.GetString(0),
                            DsModelo = reader.GetString(1),
                            NuTotalTokensPrompt = Convert.ToInt64(reader.GetValue(2)),
                            NuTotalTokensCompletion = Convert.ToInt64(reader.GetValue(3)),
                            NuTotalTokensTotal = Convert.ToInt64(reader.GetValue(4)),
                            NuCostoIngresoUsd = costoIngreso,
                            NuCostoSalidaUsd = costoSalida,
                            NuCostoTotalUsd = costoIngreso + costoSalida,
                            NuTotalArchivos = Convert.ToInt32(reader.GetValue(7))
                        });
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    var logDAL = new LogDAL();
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.ERROR,
                        DsModulo = "TokenDAL.ObtenerResumenPorMesYModelo",
                        DsMensaje = $"Error al obtener el resumen de consumo de tokens por mes y modelo: {ex.Message}",
                        DsExcepcion = ex.ToString()
                    });
                    throw;
                }
            }
        }
    }
