using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    public class RolDAL
    {
        /// <summary>
        /// Obtiene todos los roles activos del sistema
        /// </summary>
        public static List<Rol> ObtenerActivos()
        {
            List<Rol> roles = new List<Rol>();

            using (SqlConnection conn = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"
                    SELECT 
                        r.idRol,
                        r.cdRol,
                        r.dsRol,
                        r.cdEstado,
                        e.dsEstado
                    FROM TD_ROLES r
                    INNER JOIN TD_ESTADOS e ON e.dsProceso = 'ROLES' AND e.cdEstado = r.cdEstado
                    WHERE r.cdEstado = 1
                    ORDER BY r.dsRol";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Rol
                        {
                            IdRol = reader.GetInt32(0),
                            CdRol = reader.GetString(1),
                            DsRol = reader.GetString(2),
                            CdEstado = reader.GetInt32(3),
                            DsEstado = reader.GetString(4)
                        });
                    }
                }
            }

            return roles;
        }

        /// <summary>
        /// Obtiene todos los roles (activos e inactivos)
        /// </summary>
        public static List<Rol> ObtenerTodos()
        {
            List<Rol> roles = new List<Rol>();

            using (SqlConnection conn = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"
                    SELECT 
                        r.idRol,
                        r.cdRol,
                        r.dsRol,
                        r.cdEstado,
                        e.dsEstado
                    FROM TD_ROLES r
                    INNER JOIN TD_ESTADOS e ON e.dsProceso = 'ROLES' AND e.cdEstado = r.cdEstado
                    ORDER BY r.dsRol";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Rol
                        {
                            IdRol = reader.GetInt32(0),
                            CdRol = reader.GetString(1),
                            DsRol = reader.GetString(2),
                            CdEstado = reader.GetInt32(3),
                            DsEstado = reader.GetString(4)
                        });
                    }
                }
            }

            return roles;
        }
    }
}
