using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    public class LogDAL
    {
        private readonly string _cadenaConexion;

        public LogDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Registra un log en la base de datos
        /// </summary>
        public long Insertar(LogRegistro log)
        {
            try
            {
                using (var conexion = new SqlConnection(_cadenaConexion))
                {
                    var comando = new SqlCommand(
                        @"INSERT INTO TD_LOGS 
                          (dsNivel, dsModulo, dsMensaje, dsExcepcion, cdUsuario, dsUsuario, feRegistro)
                          OUTPUT INSERTED.cdLog
                          VALUES 
                          (@dsNivel, @dsModulo, @dsMensaje, @dsExcepcion, @cdUsuario, @dsUsuario, GETDATE())", conexion);

                    comando.Parameters.AddWithValue("@dsNivel", log.DsNivel);
                    comando.Parameters.AddWithValue("@dsModulo", (object?)log.DsModulo ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@dsMensaje", log.DsMensaje);
                    comando.Parameters.AddWithValue("@dsExcepcion", (object?)log.DsExcepcion ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@cdUsuario", (object?)log.CdUsuario ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@dsUsuario", (object?)log.DsUsuario ?? DBNull.Value);

                    conexion.Open();
                    var resultado = comando.ExecuteScalar();
                    return resultado != null ? Convert.ToInt64(resultado) : 0;
                }
            }
            catch
            {
                // Si falla el log, no propagar excepción para no interrumpir flujo principal
                return 0;
            }
        }

        /// <summary>
        /// Método helper para registrar logs de forma simple
        /// </summary>
        public static void RegistrarLog(string nivel, string modulo, string mensaje, Exception? excepcion = null, int? cdUsuario = null, string? dsUsuario = null)
        {
            try
            {
                var logDAL = new LogDAL();
                var log = new LogRegistro
                {
                    DsNivel = nivel,
                    DsModulo = modulo,
                    DsMensaje = mensaje,
                    DsExcepcion = excepcion != null ? $"{excepcion.Message}\n{excepcion.StackTrace}" : null,
                    CdUsuario = cdUsuario,
                    DsUsuario = dsUsuario
                };

                logDAL.Insertar(log);
            }
            catch
            {
                // Fallar silenciosamente si no se puede registrar el log
            }
        }

        /// <summary>
        /// Obtiene los últimos logs registrados
        /// </summary>
        public List<LogRegistro> ObtenerUltimos(int cantidad = 100, string? nivel = null, string? modulo = null)
        {
            var logs = new List<LogRegistro>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var sql = @"SELECT TOP (@cantidad) 
                              cdLog, dsNivel, dsModulo, dsMensaje, dsExcepcion, 
                              cdUsuario, dsUsuario, feRegistro
                            FROM TD_LOGS
                            WHERE 1=1";

                if (!string.IsNullOrEmpty(nivel))
                    sql += " AND dsNivel = @nivel";

                if (!string.IsNullOrEmpty(modulo))
                    sql += " AND dsModulo = @modulo";

                sql += " ORDER BY feRegistro DESC";

                var comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@cantidad", cantidad);

                if (!string.IsNullOrEmpty(nivel))
                    comando.Parameters.AddWithValue("@nivel", nivel);

                if (!string.IsNullOrEmpty(modulo))
                    comando.Parameters.AddWithValue("@modulo", modulo);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        logs.Add(new LogRegistro
                        {
                            CdLog = lector.GetInt64(0),
                            DsNivel = lector.GetString(1),
                            DsModulo = lector.IsDBNull(2) ? null : lector.GetString(2),
                            DsMensaje = lector.GetString(3),
                            DsExcepcion = lector.IsDBNull(4) ? null : lector.GetString(4),
                            CdUsuario = lector.IsDBNull(5) ? null : lector.GetInt32(5),
                            DsUsuario = lector.IsDBNull(6) ? null : lector.GetString(6),
                            FeRegistro = lector.GetDateTime(7)
                        });
                    }
                }
            }

            return logs;
        }
    }
}
