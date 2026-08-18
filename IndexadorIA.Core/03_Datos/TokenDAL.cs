using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

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
                    (cdResultado, nuTokensPrompt, nuTokensCompletion, nuTokensTotal, feAlta)
                    VALUES 
                    (@cdResultado, @nuTokensPrompt, @nuTokensCompletion, @nuTokensTotal, GETDATE())", conn);

                cmd.Parameters.AddWithValue("@cdResultado", token.CdResultado);
                cmd.Parameters.AddWithValue("@nuTokensPrompt", (object?)token.NuTokensPrompt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuTokensCompletion", (object?)token.NuTokensCompletion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuTokensTotal", (object?)token.NuTokensTotal ?? DBNull.Value);

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
    }
}
