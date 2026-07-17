using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Clase de acceso a datos para archivos
    /// </summary>
    public class ArchivoDAL
    {
        /// <summary>
        /// Obtiene todos los archivos de un proyecto
        /// </summary>
        public List<Archivo> ObtenerPorProyecto(int cdProyecto)
        {
            List<Archivo> archivos = new List<Archivo>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdArchivo, cdProyecto, dsNombreOriginal, dsRutaCompleta, dsTipoArchivo,
                                nuTamanoBytes, nuPaginasTotal, cdEstado, feIngreso, cdUsuarioIngreso,
                                feProcesamiento, dsObservaciones
                                FROM TD_ARCHIVOS 
                                WHERE cdProyecto = @cdProyecto
                                ORDER BY feIngreso DESC";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            archivos.Add(MapearArchivo(reader));
                        }
                    }
                }
            }

            return archivos;
        }

        /// <summary>
        /// Obtiene todos los archivos
        /// </summary>
        public List<Archivo> ObtenerTodos()
        {
            List<Archivo> archivos = new List<Archivo>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdArchivo, cdProyecto, dsNombreOriginal, dsRutaCompleta, dsTipoArchivo,
                                nuTamanoBytes, nuPaginasTotal, cdEstado, feIngreso, cdUsuarioIngreso,
                                feProcesamiento, dsObservaciones
                                FROM TD_ARCHIVOS 
                                ORDER BY feIngreso DESC";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            archivos.Add(MapearArchivo(reader));
                        }
                    }
                }
            }

            return archivos;
        }

        /// <summary>
        /// Obtiene un archivo por su código
        /// </summary>
        public Archivo? ObtenerPorCodigo(int cdArchivo)
        {
            Archivo? archivo = null;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdArchivo, cdProyecto, dsNombreOriginal, dsRutaCompleta, dsTipoArchivo,
                                nuTamanoBytes, nuPaginasTotal, cdEstado, feIngreso, cdUsuarioIngreso,
                                feProcesamiento, dsObservaciones
                                FROM TD_ARCHIVOS 
                                WHERE cdArchivo = @cdArchivo";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdArchivo", cdArchivo);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            archivo = MapearArchivo(reader);
                        }
                    }
                }
            }

            return archivo;
        }

        /// <summary>
        /// Obtiene archivos por estado
        /// </summary>
        public List<Archivo> ObtenerPorEstado(int cdEstado)
        {
            List<Archivo> archivos = new List<Archivo>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdArchivo, cdProyecto, dsNombreOriginal, dsRutaCompleta, dsTipoArchivo,
                                nuTamanoBytes, nuPaginasTotal, cdEstado, feIngreso, cdUsuarioIngreso,
                                feProcesamiento, dsObservaciones
                                FROM TD_ARCHIVOS 
                                WHERE cdEstado = @cdEstado
                                ORDER BY feIngreso DESC";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdEstado", cdEstado);
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            archivos.Add(MapearArchivo(reader));
                        }
                    }
                }
            }

            return archivos;
        }

        /// <summary>
        /// Inserta un nuevo archivo
        /// </summary>
        public int Insertar(Archivo archivo)
        {
            int nuevoId = 0;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"INSERT INTO TD_ARCHIVOS 
                                (cdProyecto, dsNombreOriginal, dsRutaCompleta, dsTipoArchivo, nuTamanoBytes,
                                nuPaginasTotal, cdEstado, feIngreso, cdUsuarioIngreso, dsObservaciones)
                                VALUES 
                                (@cdProyecto, @dsNombreOriginal, @dsRutaCompleta, @dsTipoArchivo, @nuTamanoBytes,
                                @nuPaginasTotal, @cdEstado, @feIngreso, @cdUsuarioIngreso, @dsObservaciones);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    AgregarParametros(comando, archivo);
                    conexion.Open();
                    nuevoId = (int)comando.ExecuteScalar();
                }
            }

            return nuevoId;
        }

        /// <summary>
        /// Inserta múltiples archivos en una transacción
        /// </summary>
        public int InsertarMultiples(List<Archivo> archivos)
        {
            int insertados = 0;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string query = @"INSERT INTO TD_ARCHIVOS 
                                        (cdProyecto, dsNombreOriginal, dsRutaCompleta, dsTipoArchivo, nuTamanoBytes,
                                        nuPaginasTotal, cdEstado, feIngreso, cdUsuarioIngreso, dsObservaciones)
                                        VALUES 
                                        (@cdProyecto, @dsNombreOriginal, @dsRutaCompleta, @dsTipoArchivo, @nuTamanoBytes,
                                        @nuPaginasTotal, @cdEstado, @feIngreso, @cdUsuarioIngreso, @dsObservaciones)";

                        foreach (var archivo in archivos)
                        {
                            using (SqlCommand comando = new SqlCommand(query, conexion, transaccion))
                            {
                                AgregarParametros(comando, archivo);
                                insertados += comando.ExecuteNonQuery();
                            }
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }

            return insertados;
        }

        /// <summary>
        /// Actualiza un archivo existente
        /// </summary>
        public bool Actualizar(Archivo archivo)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_ARCHIVOS SET 
                                dsNombreOriginal = @dsNombreOriginal,
                                dsRutaCompleta = @dsRutaCompleta,
                                dsTipoArchivo = @dsTipoArchivo,
                                nuTamanoBytes = @nuTamanoBytes,
                                nuPaginasTotal = @nuPaginasTotal,
                                cdEstado = @cdEstado,
                                feProcesamiento = @feProcesamiento,
                                dsObservaciones = @dsObservaciones
                                WHERE cdArchivo = @cdArchivo";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdArchivo", archivo.CdArchivo);
                    AgregarParametros(comando, archivo);
                    comando.Parameters.AddWithValue("@feProcesamiento", 
                        archivo.FeProcesamiento ?? (object)DBNull.Value);

                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Cambia el estado de un archivo
        /// </summary>
        public bool CambiarEstado(int cdArchivo, int nuevoEstado, string? observaciones = null)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_ARCHIVOS SET 
                                cdEstado = @cdEstado,
                                feProcesamiento = CASE WHEN @cdEstado IN (3, 9) THEN GETDATE() ELSE feProcesamiento END,
                                dsObservaciones = ISNULL(@dsObservaciones, dsObservaciones)
                                WHERE cdArchivo = @cdArchivo";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdArchivo", cdArchivo);
                    comando.Parameters.AddWithValue("@cdEstado", nuevoEstado);
                    comando.Parameters.AddWithValue("@dsObservaciones", observaciones ?? (object)DBNull.Value);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Elimina un archivo (físico, no lógico)
        /// </summary>
        public bool Eliminar(int cdArchivo)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = "DELETE FROM TD_ARCHIVOS WHERE cdArchivo = @cdArchivo";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdArchivo", cdArchivo);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Obtiene estadísticas de archivos por proyecto
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticas(int cdProyecto)
        {
            var estadisticas = new Dictionary<string, int>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT 
                                COUNT(*) as Total,
                                SUM(CASE WHEN cdEstado = 1 THEN 1 ELSE 0 END) as Pendientes,
                                SUM(CASE WHEN cdEstado = 2 THEN 1 ELSE 0 END) as Procesando,
                                SUM(CASE WHEN cdEstado = 3 THEN 1 ELSE 0 END) as Completados,
                                SUM(CASE WHEN cdEstado = 9 THEN 1 ELSE 0 END) as Errores,
                                SUM(nuPaginasTotal) as TotalPaginas
                                FROM TD_ARCHIVOS 
                                WHERE cdProyecto = @cdProyecto";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            estadisticas["Total"] = reader.GetInt32(0);
                            estadisticas["Pendientes"] = reader.GetInt32(1);
                            estadisticas["Procesando"] = reader.GetInt32(2);
                            estadisticas["Completados"] = reader.GetInt32(3);
                            estadisticas["Errores"] = reader.GetInt32(4);
                            estadisticas["TotalPaginas"] = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                        }
                    }
                }
            }

            return estadisticas;
        }

        /// <summary>
        /// Mapea un SqlDataReader a un objeto Archivo
        /// </summary>
        private Archivo MapearArchivo(SqlDataReader reader)
        {
            return new Archivo
            {
                CdArchivo = reader.GetInt32(0),
                CdProyecto = reader.GetInt32(1),
                DsNombreOriginal = reader.GetString(2),
                DsRutaCompleta = reader.GetString(3),
                DsTipoArchivo = reader.GetString(4),
                NuTamanoBytes = reader.GetInt64(5),
                NuPaginasTotal = reader.GetInt32(6),
                CdEstado = reader.GetInt32(7),
                FeIngreso = reader.GetDateTime(8),
                CdUsuarioIngreso = reader.GetInt32(9),
                FeProcesamiento = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                DsObservaciones = reader.IsDBNull(11) ? null : reader.GetString(11)
            };
        }

        /// <summary>
        /// Agrega parámetros comunes a un comando
        /// </summary>
        private void AgregarParametros(SqlCommand comando, Archivo archivo)
        {
            comando.Parameters.AddWithValue("@cdProyecto", archivo.CdProyecto);
            comando.Parameters.AddWithValue("@dsNombreOriginal", archivo.DsNombreOriginal);
            comando.Parameters.AddWithValue("@dsRutaCompleta", archivo.DsRutaCompleta);
            comando.Parameters.AddWithValue("@dsTipoArchivo", archivo.DsTipoArchivo);
            comando.Parameters.AddWithValue("@nuTamanoBytes", archivo.NuTamanoBytes);
            comando.Parameters.AddWithValue("@nuPaginasTotal", archivo.NuPaginasTotal);
            comando.Parameters.AddWithValue("@cdEstado", archivo.CdEstado);
            comando.Parameters.AddWithValue("@feIngreso", archivo.FeIngreso);
            comando.Parameters.AddWithValue("@cdUsuarioIngreso", archivo.CdUsuarioIngreso);
            comando.Parameters.AddWithValue("@dsObservaciones", archivo.DsObservaciones ?? (object)DBNull.Value);
        }
    }
}
