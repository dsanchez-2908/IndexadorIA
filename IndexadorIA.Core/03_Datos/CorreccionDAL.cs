using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_CORRECIONES, que registra
    /// correcciones manuales de campos de resultados de IA con fines estadísticos.
    /// </summary>
    public class CorreccionDAL
    {
        private readonly string _cadenaConexion;

        public CorreccionDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Inserta un registro de corrección manual.
        /// </summary>
        public void Insertar(Correccion correccion)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    INSERT INTO TD_CORRECIONES
                    (cdResultado, dsCampo, dsValorAnterior, dsValorNuevo, feControl, cdUsuarioControl)
                    VALUES
                    (@cdResultado, @dsCampo, @dsValorAnterior, @dsValorNuevo, GETDATE(), @cdUsuarioControl)", conn);

                cmd.Parameters.AddWithValue("@cdResultado", correccion.CdResultado);
                cmd.Parameters.AddWithValue("@dsCampo", correccion.DsCampo);
                cmd.Parameters.AddWithValue("@dsValorAnterior", (object?)correccion.DsValorAnterior ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsValorNuevo", (object?)correccion.DsValorNuevo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cdUsuarioControl", correccion.CdUsuarioControl);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "CorreccionDAL.Insertar",
                    DsMensaje = $"Error al insertar corrección para resultado {correccion.CdResultado}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
    }
}
