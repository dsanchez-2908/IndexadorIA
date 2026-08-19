using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Clase de acceso a datos para usuarios
    /// </summary>
    public class UsuarioDAL
    {
        /// <summary>
        /// Valida las credenciales de un usuario
        /// </summary>
        public Usuario? ValidarLogin(string dsUsuario, string dsClave)
        {
            Usuario? usuario = null;
            string claveEncriptada = Seguridad.EncriptarSHA256(dsClave);

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("SP_VALIDAR_LOGIN", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@dsUsuario", dsUsuario);
                    comando.Parameters.AddWithValue("@dsClave", claveEncriptada);

                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                CdUsuario = reader.GetInt32(0),
                                DsUsuario = reader.GetString(1),
                                DsNombreCompleto = reader.GetString(2),
                                SnClaveTemporal = reader.GetBoolean(3),
                                SnPrimerIngreso = reader.GetBoolean(4),
                                IdRol = reader.GetInt32(5),
                                CdRol = reader.GetString(6),
                                DsRol = reader.GetString(7),
                                CdEstado = reader.GetInt32(8),
                                DsEstado = reader.GetString(9)
                            };
                        }
                    }
                }
            }

            return usuario;
        }

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT 
                                    u.cdUsuario, u.dsUsuario, u.dsClave, u.dsNombreCompleto, 
                                    u.snClaveTemporal, u.snPrimerIngreso, 
                                    u.idRol, r.cdRol, r.dsRol,
                                    u.cdEstado, e.dsEstado,
                                    u.feAlta, u.cdUsuarioAlta
                                FROM TD_USUARIOS u
                                INNER JOIN TD_ROLES r ON u.idRol = r.idRol
                                INNER JOIN TD_ESTADOS e ON e.dsProceso = 'USUARIOS' AND e.cdEstado = u.cdEstado
                                ORDER BY u.dsNombreCompleto";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                CdUsuario = reader.GetInt32(0),
                                DsUsuario = reader.GetString(1),
                                DsClave = reader.GetString(2),
                                DsNombreCompleto = reader.GetString(3),
                                SnClaveTemporal = reader.GetBoolean(4),
                                SnPrimerIngreso = reader.GetBoolean(5),
                                IdRol = reader.GetInt32(6),
                                CdRol = reader.GetString(7),
                                DsRol = reader.GetString(8),
                                CdEstado = reader.GetInt32(9),
                                DsEstado = reader.GetString(10),
                                FeAlta = reader.GetDateTime(11),
                                CdUsuarioAlta = reader.IsDBNull(12) ? null : reader.GetInt32(12)
                            });
                        }
                    }
                }
            }

            return usuarios;
        }

        /// <summary>
        /// Obtiene un usuario por su código
        /// </summary>
        public Usuario? ObtenerPorCodigo(int cdUsuario)
        {
            Usuario? usuario = null;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT 
                                    u.cdUsuario, u.dsUsuario, u.dsClave, u.dsNombreCompleto, 
                                    u.snClaveTemporal, u.snPrimerIngreso,
                                    u.idRol, r.cdRol, r.dsRol,
                                    u.cdEstado, e.dsEstado,
                                    u.feAlta, u.cdUsuarioAlta
                                FROM TD_USUARIOS u
                                INNER JOIN TD_ROLES r ON u.idRol = r.idRol
                                INNER JOIN TD_ESTADOS e ON e.dsProceso = 'USUARIOS' AND e.cdEstado = u.cdEstado
                                WHERE u.cdUsuario = @cdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdUsuario", cdUsuario);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                CdUsuario = reader.GetInt32(0),
                                DsUsuario = reader.GetString(1),
                                DsClave = reader.GetString(2),
                                DsNombreCompleto = reader.GetString(3),
                                SnClaveTemporal = reader.GetBoolean(4),
                                SnPrimerIngreso = reader.GetBoolean(5),
                                IdRol = reader.GetInt32(6),
                                CdRol = reader.GetString(7),
                                DsRol = reader.GetString(8),
                                CdEstado = reader.GetInt32(9),
                                DsEstado = reader.GetString(10),
                                FeAlta = reader.GetDateTime(11),
                                CdUsuarioAlta = reader.IsDBNull(12) ? null : reader.GetInt32(12)
                            };
                        }
                    }
                }
            }

            return usuario;
        }

        /// <summary>
        /// Inserta un nuevo usuario
        /// </summary>
        public int Insertar(Usuario usuario)
        {
            int nuevoId = 0;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"INSERT INTO TD_USUARIOS 
                                (dsUsuario, dsClave, dsNombreCompleto, snClaveTemporal, 
                                snPrimerIngreso, idRol, cdEstado, feAlta, cdUsuarioAlta)
                                VALUES 
                                (@dsUsuario, @dsClave, @dsNombreCompleto, @snClaveTemporal, 
                                @snPrimerIngreso, @idRol, @cdEstado, @feAlta, @cdUsuarioAlta);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@dsUsuario", usuario.DsUsuario);
                    comando.Parameters.AddWithValue("@dsClave", usuario.DsClave);
                    comando.Parameters.AddWithValue("@dsNombreCompleto", usuario.DsNombreCompleto);
                    comando.Parameters.AddWithValue("@snClaveTemporal", usuario.SnClaveTemporal);
                    comando.Parameters.AddWithValue("@snPrimerIngreso", usuario.SnPrimerIngreso);
                    comando.Parameters.AddWithValue("@idRol", usuario.IdRol);
                    comando.Parameters.AddWithValue("@cdEstado", usuario.CdEstado);
                    comando.Parameters.AddWithValue("@feAlta", usuario.FeAlta);
                    comando.Parameters.AddWithValue("@cdUsuarioAlta", 
                        usuario.CdUsuarioAlta.HasValue ? (object)usuario.CdUsuarioAlta.Value : DBNull.Value);

                    conexion.Open();
                    nuevoId = (int)comando.ExecuteScalar();
                }
            }

            return nuevoId;
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        public bool Actualizar(Usuario usuario)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_USUARIOS SET 
                                dsUsuario = @dsUsuario,
                                dsClave = @dsClave,
                                dsNombreCompleto = @dsNombreCompleto,
                                snClaveTemporal = @snClaveTemporal,
                                snPrimerIngreso = @snPrimerIngreso,
                                idRol = @idRol,
                                cdEstado = @cdEstado
                                WHERE cdUsuario = @cdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdUsuario", usuario.CdUsuario);
                    comando.Parameters.AddWithValue("@dsUsuario", usuario.DsUsuario);
                    comando.Parameters.AddWithValue("@dsClave", usuario.DsClave);
                    comando.Parameters.AddWithValue("@dsNombreCompleto", usuario.DsNombreCompleto);
                    comando.Parameters.AddWithValue("@snClaveTemporal", usuario.SnClaveTemporal);
                    comando.Parameters.AddWithValue("@snPrimerIngreso", usuario.SnPrimerIngreso);
                    comando.Parameters.AddWithValue("@idRol", usuario.IdRol);
                    comando.Parameters.AddWithValue("@cdEstado", usuario.CdEstado);

                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Elimina un usuario (cambio lógico de estado)
        /// </summary>
        public bool Eliminar(int cdUsuario)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = "UPDATE TD_USUARIOS SET cdEstado = 0 WHERE cdUsuario = @cdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdUsuario", cdUsuario);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Cambia la contraseña de un usuario
        /// </summary>
        public bool CambiarClave(int cdUsuario, string nuevaClave, bool esClaveTemporal = false)
        {
            string claveEncriptada = Seguridad.EncriptarSHA256(nuevaClave);

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_USUARIOS SET 
                                dsClave = @dsClave,
                                snClaveTemporal = @snClaveTemporal,
                                snPrimerIngreso = 0
                                WHERE cdUsuario = @cdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdUsuario", cdUsuario);
                    comando.Parameters.AddWithValue("@dsClave", claveEncriptada);
                    comando.Parameters.AddWithValue("@snClaveTemporal", esClaveTemporal);

                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Verifica si existe un usuario con el nombre especificado
        /// </summary>
        public bool ExisteUsuario(string dsUsuario, int? cdUsuarioExcluir = null)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = "SELECT COUNT(*) FROM TD_USUARIOS WHERE dsUsuario = @dsUsuario";

                if (cdUsuarioExcluir.HasValue)
                {
                    query += " AND cdUsuario != @cdUsuarioExcluir";
                }

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@dsUsuario", dsUsuario);
                    if (cdUsuarioExcluir.HasValue)
                    {
                        comando.Parameters.AddWithValue("@cdUsuarioExcluir", cdUsuarioExcluir.Value);
                    }

                    conexion.Open();
                    return (int)comando.ExecuteScalar() > 0;
                }
            }
        }
    }
}
