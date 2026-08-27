using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_001_RESULTADO_IA_ERROR.
    /// Registra los fallos de procesamiento de resultados de OpenAI
    /// (error de la API o excepcion al parsear el JSON) para poder
    /// identificar y reprocesar las paginas afectadas.
    /// </summary>
    public class ResultadoIAErrorDAL
    {
        private readonly string _cadenaConexion;

        public ResultadoIAErrorDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Inserta un registro de error. Si ya existiera un registro previo
        /// para la misma pagina (por ejemplo, al reprocesar un batch), se
        /// elimina antes de insertar el nuevo para evitar duplicados.
        /// </summary>
        public void Insertar(ResultadoIAError error)
        {
            using var conn = new SqlConnection(_cadenaConexion);
            conn.Open();

            using (var cmdDelete = new SqlCommand(
                "DELETE FROM TD_001_RESULTADO_IA_ERROR WHERE cdArchivoPagina = @cdArchivoPagina", conn))
            {
                cmdDelete.Parameters.AddWithValue("@cdArchivoPagina", error.CdArchivoPagina);
                cmdDelete.ExecuteNonQuery();
            }

            using var cmd = new SqlCommand(@"
                INSERT INTO TD_001_RESULTADO_IA_ERROR
                (cdLote, cdArchivoPagina, dsMotivoError, dsRespuestaCruda, feAlta)
                VALUES
                (@cdLote, @cdArchivoPagina, @dsMotivoError, @dsRespuestaCruda, GETDATE());", conn);

            cmd.Parameters.AddWithValue("@cdLote", error.CdLote);
            cmd.Parameters.AddWithValue("@cdArchivoPagina", error.CdArchivoPagina);
            cmd.Parameters.AddWithValue("@dsMotivoError", error.DsMotivoError);
            cmd.Parameters.AddWithValue("@dsRespuestaCruda", (object?)error.DsRespuestaCruda ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }
}
