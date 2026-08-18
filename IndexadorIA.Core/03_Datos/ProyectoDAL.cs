using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    public class ProyectoDAL
    {
        private readonly string _cadenaConexion;

        public ProyectoDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        public List<Proyecto> ObtenerActivos()
        {
            var proyectos = new List<Proyecto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    @"SELECT cdProyecto, dsProyecto, snActivo, feAlta, cdUsuarioAlta, 
                             feUltimaModificacion, cdUsuarioModificacion
                      FROM TD_PROYECTOS 
                      WHERE snActivo = 1
                      ORDER BY dsProyecto", conexion);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        proyectos.Add(new Proyecto
                        {
                            CdProyecto = lector.GetInt32(0),
                            DsProyecto = lector.GetString(1),
                            SnActivo = lector.GetBoolean(2),
                            FeAlta = lector.GetDateTime(3),
                            CdUsuarioAlta = lector.GetInt32(4),
                            FeUltimaModificacion = lector.IsDBNull(5) ? null : lector.GetDateTime(5),
                            CdUsuarioModificacion = lector.IsDBNull(6) ? null : lector.GetInt32(6)
                        });
                    }
                }
            }

            return proyectos;
        }

        public List<Proyecto> ObtenerTodos()
        {
            var proyectos = new List<Proyecto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    @"SELECT cdProyecto, dsProyecto, snActivo, feAlta, cdUsuarioAlta, 
                             feUltimaModificacion, cdUsuarioModificacion
                      FROM TD_PROYECTOS 
                      ORDER BY dsProyecto", conexion);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        proyectos.Add(new Proyecto
                        {
                            CdProyecto = lector.GetInt32(0),
                            DsProyecto = lector.GetString(1),
                            SnActivo = lector.GetBoolean(2),
                            FeAlta = lector.GetDateTime(3),
                            CdUsuarioAlta = lector.GetInt32(4),
                            FeUltimaModificacion = lector.IsDBNull(5) ? null : lector.GetDateTime(5),
                            CdUsuarioModificacion = lector.IsDBNull(6) ? null : lector.GetInt32(6)
                        });
                    }
                }
            }

            return proyectos;
        }

        public Proyecto? ObtenerPorCodigo(int cdProyecto)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    @"SELECT cdProyecto, dsProyecto, snActivo, feAlta, cdUsuarioAlta, 
                             feUltimaModificacion, cdUsuarioModificacion
                      FROM TD_PROYECTOS 
                      WHERE cdProyecto = @cdProyecto", conexion);

                comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return new Proyecto
                        {
                            CdProyecto = lector.GetInt32(0),
                            DsProyecto = lector.GetString(1),
                            SnActivo = lector.GetBoolean(2),
                            FeAlta = lector.GetDateTime(3),
                            CdUsuarioAlta = lector.GetInt32(4),
                            FeUltimaModificacion = lector.IsDBNull(5) ? null : lector.GetDateTime(5),
                            CdUsuarioModificacion = lector.IsDBNull(6) ? null : lector.GetInt32(6)
                        };
                    }
                }
            }

            return null;
        }

        public int Insertar(string dsProyecto, bool snActivo, int cdUsuarioAlta)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    @"INSERT INTO TD_PROYECTOS (dsProyecto, snActivo, feAlta, cdUsuarioAlta)
                      VALUES (@dsProyecto, @snActivo, GETDATE(), @cdUsuarioAlta);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);", conexion);

                comando.Parameters.AddWithValue("@dsProyecto", dsProyecto);
                comando.Parameters.AddWithValue("@snActivo", snActivo);
                comando.Parameters.AddWithValue("@cdUsuarioAlta", cdUsuarioAlta);

                conexion.Open();
                return (int)comando.ExecuteScalar();
            }
        }

        public void Actualizar(int cdProyecto, string dsProyecto, bool snActivo, int cdUsuarioModificacion)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    @"UPDATE TD_PROYECTOS 
                      SET dsProyecto = @dsProyecto,
                          snActivo = @snActivo,
                          feUltimaModificacion = GETDATE(),
                          cdUsuarioModificacion = @cdUsuarioModificacion
                      WHERE cdProyecto = @cdProyecto", conexion);

                comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);
                comando.Parameters.AddWithValue("@dsProyecto", dsProyecto);
                comando.Parameters.AddWithValue("@snActivo", snActivo);
                comando.Parameters.AddWithValue("@cdUsuarioModificacion", cdUsuarioModificacion);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public void Eliminar(int cdProyecto)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    "DELETE FROM TD_PROYECTOS WHERE cdProyecto = @cdProyecto", conexion);

                comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public bool ExisteProyecto(string dsProyecto, int? cdProyectoExcluir = null)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var sql = "SELECT COUNT(*) FROM TD_PROYECTOS WHERE dsProyecto = @dsProyecto";

                if (cdProyectoExcluir.HasValue)
                {
                    sql += " AND cdProyecto != @cdProyectoExcluir";
                }

                var comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@dsProyecto", dsProyecto);

                if (cdProyectoExcluir.HasValue)
                {
                    comando.Parameters.AddWithValue("@cdProyectoExcluir", cdProyectoExcluir.Value);
                }

                conexion.Open();
                return (int)comando.ExecuteScalar() > 0;
            }
        }
    }
}
