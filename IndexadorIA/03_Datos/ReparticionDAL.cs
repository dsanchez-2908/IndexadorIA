using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_REPARTICIONES
    /// </summary>
    public class ReparticionDAL
    {
        private readonly string _cadenaConexion;

        public ReparticionDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene todas las reparticiones registradas
        /// </summary>
        public List<Reparticion> ObtenerTodos()
        {
            List<Reparticion> reparticiones = new();

            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    SELECT cdReparticion, dsReparticion 
                    FROM TD_REPARTICIONES 
                    ORDER BY dsReparticion", conn);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reparticiones.Add(new Reparticion
                    {
                        CdReparticion = reader.GetInt32(0),
                        DsReparticion = reader.GetString(1)
                    });
                }

                return reparticiones;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "ReparticionDAL.ObtenerTodos",
                    DsMensaje = $"Error al obtener reparticiones: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
    }
}
