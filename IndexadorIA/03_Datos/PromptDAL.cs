using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_PROMPT
    /// </summary>
    public class PromptDAL
    {
        private readonly string _cadenaConexion;

        public PromptDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene el prompt activo para un proyecto
        /// </summary>
        public Prompt? ObtenerPorProyecto(int cdProyecto)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    SELECT cdPrompt, cdProyecto, dsDescripcion, dsPrompt, 
                           feAlta, cdUsuarioAlta, feUltimaModificacion, cdUsuarioModificacion 
                    FROM TD_PROMPT 
                    WHERE cdProyecto = @cdProyecto", conn);

                cmd.Parameters.AddWithValue("@cdProyecto", cdProyecto);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Prompt
                    {
                        CdPrompt = reader.GetInt32(0),
                        CdProyecto = reader.GetInt32(1),
                        DsDescripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                        DsPrompt = reader.GetString(3),
                        FeAlta = reader.GetDateTime(4),
                        CdUsuarioAlta = reader.GetInt32(5),
                        FeUltimaModificacion = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                        CdUsuarioModificacion = reader.IsDBNull(7) ? null : reader.GetInt32(7)
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PromptDAL.ObtenerPorProyecto",
                    DsMensaje = $"Error al obtener prompt del proyecto {cdProyecto}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Actualiza el texto del prompt
        /// </summary>
        public void ActualizarPrompt(int cdPrompt, string dsPrompt, int cdUsuarioModificacion)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    UPDATE TD_PROMPT 
                    SET dsPrompt = @dsPrompt, 
                        feUltimaModificacion = GETDATE(), 
                        cdUsuarioModificacion = @cdUsuarioModificacion 
                    WHERE cdPrompt = @cdPrompt", conn);

                cmd.Parameters.AddWithValue("@dsPrompt", dsPrompt);
                cmd.Parameters.AddWithValue("@cdUsuarioModificacion", cdUsuarioModificacion);
                cmd.Parameters.AddWithValue("@cdPrompt", cdPrompt);

                conn.Open();
                cmd.ExecuteNonQuery();

                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "PromptDAL.ActualizarPrompt",
                    DsMensaje = $"Prompt {cdPrompt} actualizado correctamente"
                });
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PromptDAL.ActualizarPrompt",
                    DsMensaje = $"Error al actualizar prompt {cdPrompt}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
    }
}
