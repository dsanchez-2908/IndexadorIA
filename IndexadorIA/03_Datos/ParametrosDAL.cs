using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_PARAMETROS
    /// </summary>
    public class ParametrosDAL
    {
        private readonly string _cadenaConexion;

        public ParametrosDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene el valor de un parámetro por su clave
        /// </summary>
        public string? ObtenerValor(string clave)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand("SELECT dsValorParametro FROM TD_PARAMETROS WHERE dsClaveParametro = @clave", conn);

                cmd.Parameters.AddWithValue("@clave", clave);

                conn.Open();
                object? result = cmd.ExecuteScalar();

                return result?.ToString();
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new Entidades.LogRegistro
                {
                    DsNivel = Entidades.LogRegistro.Niveles.ERROR,
                    DsModulo = "ParametrosDAL.ObtenerValor",
                    DsMensaje = $"Error al obtener parámetro '{clave}': {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Actualiza el valor de un parámetro
        /// </summary>
        public void ActualizarValor(string clave, string valor, int cdUsuario)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    UPDATE TD_PARAMETROS 
                    SET dsValorParametro = @valor, 
                        feUltimaModificacion = GETDATE(), 
                        cdUsuarioModificacion = @cdUsuario
                    WHERE dsClaveParametro = @clave", conn);

                cmd.Parameters.AddWithValue("@clave", clave);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@cdUsuario", cdUsuario);

                conn.Open();
                cmd.ExecuteNonQuery();

                var logDAL = new LogDAL();
                logDAL.Insertar(new Entidades.LogRegistro
                {
                    DsNivel = Entidades.LogRegistro.Niveles.INFO,
                    DsModulo = "ParametrosDAL.ActualizarValor",
                    DsMensaje = $"Parámetro '{clave}' actualizado a '{valor}'"
                });
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new Entidades.LogRegistro
                {
                    DsNivel = Entidades.LogRegistro.Niveles.ERROR,
                    DsModulo = "ParametrosDAL.ActualizarValor",
                    DsMensaje = $"Error al actualizar parámetro '{clave}': {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
    }
}
