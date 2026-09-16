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
        /// Inserta un registro de corrección manual. Si <see cref="Correccion.CdUsuarioAuditoria"/>
        /// tiene valor, la corrección se registra como proveniente de la auditoría (columnas
        /// feAuditoria/cdUsuarioAuditoria); en caso contrario se registra como proveniente
        /// del control (columnas feControl/cdUsuarioControl).
        /// </summary>
        public void Insertar(Correccion correccion)
        {
            try
            {
                bool esAuditoria = correccion.CdUsuarioAuditoria.HasValue;

                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    INSERT INTO TD_CORRECIONES
                    (cdResultado, dsCampo, dsValorAnterior, dsValorNuevo, feControl, cdUsuarioControl, feAuditoria, cdUsuarioAuditoria)
                    VALUES
                    (@cdResultado, @dsCampo, @dsValorAnterior, @dsValorNuevo, @feControl, @cdUsuarioControl, @feAuditoria, @cdUsuarioAuditoria)", conn);

                cmd.Parameters.AddWithValue("@cdResultado", correccion.CdResultado);
                cmd.Parameters.AddWithValue("@dsCampo", correccion.DsCampo);
                cmd.Parameters.AddWithValue("@dsValorAnterior", (object?)correccion.DsValorAnterior ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsValorNuevo", (object?)correccion.DsValorNuevo ?? DBNull.Value);

                if (esAuditoria)
                {
                    cmd.Parameters.AddWithValue("@feControl", DBNull.Value);
                    cmd.Parameters.AddWithValue("@cdUsuarioControl", DBNull.Value);
                    cmd.Parameters.AddWithValue("@feAuditoria", DateTime.Now);
                    cmd.Parameters.AddWithValue("@cdUsuarioAuditoria", correccion.CdUsuarioAuditoria!.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@feControl", DateTime.Now);
                    cmd.Parameters.AddWithValue("@cdUsuarioControl", (object?)correccion.CdUsuarioControl ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@feAuditoria", DBNull.Value);
                    cmd.Parameters.AddWithValue("@cdUsuarioAuditoria", DBNull.Value);
                }

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
