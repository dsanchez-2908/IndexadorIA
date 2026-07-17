using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Clase de acceso a datos para páginas
    /// </summary>
    public class PaginaDAL
    {
        /// <summary>
        /// Obtiene todas las páginas de un archivo
        /// </summary>
        public List<Pagina> ObtenerPorArchivo(int cdArchivo)
        {
            List<Pagina> paginas = new List<Pagina>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdPagina, cdArchivo, nuNumeroPagina, dsRutaImagenOriginal,
                                dsRutaImagenRotada, dsRutaImagenRecortada, nuGradosRotacion, snRotada,
                                snRecortada, nuAnchoOriginal, nuAltoOriginal, cdEstado, feCreacion,
                                feProcesamiento, dsObservaciones
                                FROM TD_PAGINAS 
                                WHERE cdArchivo = @cdArchivo
                                ORDER BY nuNumeroPagina";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdArchivo", cdArchivo);
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paginas.Add(MapearPagina(reader));
                        }
                    }
                }
            }

            return paginas;
        }

        /// <summary>
        /// Obtiene todas las páginas de un proyecto
        /// </summary>
        public List<Pagina> ObtenerPorProyecto(int cdProyecto)
        {
            List<Pagina> paginas = new List<Pagina>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT p.cdPagina, p.cdArchivo, p.nuNumeroPagina, p.dsRutaImagenOriginal,
                                p.dsRutaImagenRotada, p.dsRutaImagenRecortada, p.nuGradosRotacion, p.snRotada,
                                p.snRecortada, p.nuAnchoOriginal, p.nuAltoOriginal, p.cdEstado, p.feCreacion,
                                p.feProcesamiento, p.dsObservaciones
                                FROM TD_PAGINAS p
                                INNER JOIN TD_ARCHIVOS a ON p.cdArchivo = a.cdArchivo
                                WHERE a.cdProyecto = @cdProyecto
                                ORDER BY p.feCreacion DESC";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paginas.Add(MapearPagina(reader));
                        }
                    }
                }
            }

            return paginas;
        }

        /// <summary>
        /// Obtiene páginas por estado
        /// </summary>
        public List<Pagina> ObtenerPorEstado(int cdEstado, int? cdProyecto = null)
        {
            List<Pagina> paginas = new List<Pagina>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT p.cdPagina, p.cdArchivo, p.nuNumeroPagina, p.dsRutaImagenOriginal,
                                p.dsRutaImagenRotada, p.dsRutaImagenRecortada, p.nuGradosRotacion, p.snRotada,
                                p.snRecortada, p.nuAnchoOriginal, p.nuAltoOriginal, p.cdEstado, p.feCreacion,
                                p.feProcesamiento, p.dsObservaciones
                                FROM TD_PAGINAS p";

                if (cdProyecto.HasValue)
                {
                    query += @" INNER JOIN TD_ARCHIVOS a ON p.cdArchivo = a.cdArchivo
                               WHERE p.cdEstado = @cdEstado AND a.cdProyecto = @cdProyecto";
                }
                else
                {
                    query += " WHERE p.cdEstado = @cdEstado";
                }

                query += " ORDER BY p.feCreacion";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdEstado", cdEstado);
                    if (cdProyecto.HasValue)
                    {
                        comando.Parameters.AddWithValue("@cdProyecto", cdProyecto.Value);
                    }

                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paginas.Add(MapearPagina(reader));
                        }
                    }
                }
            }

            return paginas;
        }

        /// <summary>
        /// Obtiene una página por su código
        /// </summary>
        public Pagina? ObtenerPorCodigo(int cdPagina)
        {
            Pagina? pagina = null;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdPagina, cdArchivo, nuNumeroPagina, dsRutaImagenOriginal,
                                dsRutaImagenRotada, dsRutaImagenRecortada, nuGradosRotacion, snRotada,
                                snRecortada, nuAnchoOriginal, nuAltoOriginal, cdEstado, feCreacion,
                                feProcesamiento, dsObservaciones
                                FROM TD_PAGINAS 
                                WHERE cdPagina = @cdPagina";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdPagina", cdPagina);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pagina = MapearPagina(reader);
                        }
                    }
                }
            }

            return pagina;
        }

        /// <summary>
        /// Inserta una nueva página
        /// </summary>
        public int Insertar(Pagina pagina)
        {
            int nuevoId = 0;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"INSERT INTO TD_PAGINAS 
                                (cdArchivo, nuNumeroPagina, dsRutaImagenOriginal, dsRutaImagenRotada,
                                dsRutaImagenRecortada, nuGradosRotacion, snRotada, snRecortada,
                                nuAnchoOriginal, nuAltoOriginal, cdEstado, feCreacion, dsObservaciones)
                                VALUES 
                                (@cdArchivo, @nuNumeroPagina, @dsRutaImagenOriginal, @dsRutaImagenRotada,
                                @dsRutaImagenRecortada, @nuGradosRotacion, @snRotada, @snRecortada,
                                @nuAnchoOriginal, @nuAltoOriginal, @cdEstado, @feCreacion, @dsObservaciones);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    AgregarParametros(comando, pagina);
                    conexion.Open();
                    nuevoId = (int)comando.ExecuteScalar();
                }
            }

            return nuevoId;
        }

        /// <summary>
        /// Inserta múltiples páginas en una transacción
        /// </summary>
        public List<int> InsertarMultiples(List<Pagina> paginas)
        {
            List<int> ids = new List<int>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string query = @"INSERT INTO TD_PAGINAS 
                                        (cdArchivo, nuNumeroPagina, dsRutaImagenOriginal, dsRutaImagenRotada,
                                        dsRutaImagenRecortada, nuGradosRotacion, snRotada, snRecortada,
                                        nuAnchoOriginal, nuAltoOriginal, cdEstado, feCreacion, dsObservaciones)
                                        VALUES 
                                        (@cdArchivo, @nuNumeroPagina, @dsRutaImagenOriginal, @dsRutaImagenRotada,
                                        @dsRutaImagenRecortada, @nuGradosRotacion, @snRotada, @snRecortada,
                                        @nuAnchoOriginal, @nuAltoOriginal, @cdEstado, @feCreacion, @dsObservaciones);
                                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        foreach (var pagina in paginas)
                        {
                            using (SqlCommand comando = new SqlCommand(query, conexion, transaccion))
                            {
                                AgregarParametros(comando, pagina);
                                int nuevoId = (int)comando.ExecuteScalar();
                                ids.Add(nuevoId);
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

            return ids;
        }

        /// <summary>
        /// Actualiza una página existente
        /// </summary>
        public bool Actualizar(Pagina pagina)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_PAGINAS SET 
                                dsRutaImagenOriginal = @dsRutaImagenOriginal,
                                dsRutaImagenRotada = @dsRutaImagenRotada,
                                dsRutaImagenRecortada = @dsRutaImagenRecortada,
                                nuGradosRotacion = @nuGradosRotacion,
                                snRotada = @snRotada,
                                snRecortada = @snRecortada,
                                nuAnchoOriginal = @nuAnchoOriginal,
                                nuAltoOriginal = @nuAltoOriginal,
                                cdEstado = @cdEstado,
                                feProcesamiento = @feProcesamiento,
                                dsObservaciones = @dsObservaciones
                                WHERE cdPagina = @cdPagina";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdPagina", pagina.CdPagina);
                    AgregarParametros(comando, pagina);
                    comando.Parameters.AddWithValue("@feProcesamiento", 
                        pagina.FeProcesamiento ?? (object)DBNull.Value);

                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Actualiza la ruta de imagen rotada
        /// </summary>
        public bool ActualizarImagenRotada(int cdPagina, string rutaRotada, int gradosRotacion)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_PAGINAS SET 
                                dsRutaImagenRotada = @dsRutaImagenRotada,
                                nuGradosRotacion = @nuGradosRotacion,
                                snRotada = 1,
                                cdEstado = CASE WHEN cdEstado = 1 THEN 2 ELSE cdEstado END
                                WHERE cdPagina = @cdPagina";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdPagina", cdPagina);
                    comando.Parameters.AddWithValue("@dsRutaImagenRotada", rutaRotada);
                    comando.Parameters.AddWithValue("@nuGradosRotacion", gradosRotacion);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Actualiza la ruta de imagen recortada
        /// </summary>
        public bool ActualizarImagenRecortada(int cdPagina, string rutaRecortada)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_PAGINAS SET 
                                dsRutaImagenRecortada = @dsRutaImagenRecortada,
                                snRecortada = 1,
                                cdEstado = CASE WHEN cdEstado IN (1,2) THEN 3 ELSE cdEstado END
                                WHERE cdPagina = @cdPagina";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdPagina", cdPagina);
                    comando.Parameters.AddWithValue("@dsRutaImagenRecortada", rutaRecortada);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Cambia el estado de una página
        /// </summary>
        public bool CambiarEstado(int cdPagina, int nuevoEstado, string? observaciones = null)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_PAGINAS SET 
                                cdEstado = @cdEstado,
                                feProcesamiento = CASE WHEN @cdEstado IN (5, 9) THEN GETDATE() ELSE feProcesamiento END,
                                dsObservaciones = ISNULL(@dsObservaciones, dsObservaciones)
                                WHERE cdPagina = @cdPagina";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdPagina", cdPagina);
                    comando.Parameters.AddWithValue("@cdEstado", nuevoEstado);
                    comando.Parameters.AddWithValue("@dsObservaciones", observaciones ?? (object)DBNull.Value);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Elimina una página
        /// </summary>
        public bool Eliminar(int cdPagina)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = "DELETE FROM TD_PAGINAS WHERE cdPagina = @cdPagina";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdPagina", cdPagina);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Obtiene estadísticas de páginas por archivo
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticas(int cdArchivo)
        {
            var estadisticas = new Dictionary<string, int>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT 
                                COUNT(*) as Total,
                                SUM(CASE WHEN cdEstado = 1 THEN 1 ELSE 0 END) as Pendientes,
                                SUM(CASE WHEN cdEstado = 2 THEN 1 ELSE 0 END) as Rotadas,
                                SUM(CASE WHEN cdEstado = 3 THEN 1 ELSE 0 END) as Recortadas,
                                SUM(CASE WHEN cdEstado = 4 THEN 1 ELSE 0 END) as EnLote,
                                SUM(CASE WHEN cdEstado = 5 THEN 1 ELSE 0 END) as Procesadas,
                                SUM(CASE WHEN cdEstado = 9 THEN 1 ELSE 0 END) as Errores
                                FROM TD_PAGINAS 
                                WHERE cdArchivo = @cdArchivo";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdArchivo", cdArchivo);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            estadisticas["Total"] = reader.GetInt32(0);
                            estadisticas["Pendientes"] = reader.GetInt32(1);
                            estadisticas["Rotadas"] = reader.GetInt32(2);
                            estadisticas["Recortadas"] = reader.GetInt32(3);
                            estadisticas["EnLote"] = reader.GetInt32(4);
                            estadisticas["Procesadas"] = reader.GetInt32(5);
                            estadisticas["Errores"] = reader.GetInt32(6);
                        }
                    }
                }
            }

            return estadisticas;
        }

        /// <summary>
        /// Mapea un SqlDataReader a un objeto Pagina
        /// </summary>
        private Pagina MapearPagina(SqlDataReader reader)
        {
            return new Pagina
            {
                CdPagina = reader.GetInt32(0),
                CdArchivo = reader.GetInt32(1),
                NuNumeroPagina = reader.GetInt32(2),
                DsRutaImagenOriginal = reader.GetString(3),
                DsRutaImagenRotada = reader.IsDBNull(4) ? null : reader.GetString(4),
                DsRutaImagenRecortada = reader.IsDBNull(5) ? null : reader.GetString(5),
                NuGradosRotacion = reader.GetInt32(6),
                SnRotada = reader.GetBoolean(7),
                SnRecortada = reader.GetBoolean(8),
                NuAnchoOriginal = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                NuAltoOriginal = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                CdEstado = reader.GetInt32(11),
                FeCreacion = reader.GetDateTime(12),
                FeProcesamiento = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                DsObservaciones = reader.IsDBNull(14) ? null : reader.GetString(14)
            };
        }

        /// <summary>
        /// Agrega parámetros comunes a un comando
        /// </summary>
        private void AgregarParametros(SqlCommand comando, Pagina pagina)
        {
            comando.Parameters.AddWithValue("@cdArchivo", pagina.CdArchivo);
            comando.Parameters.AddWithValue("@nuNumeroPagina", pagina.NuNumeroPagina);
            comando.Parameters.AddWithValue("@dsRutaImagenOriginal", pagina.DsRutaImagenOriginal);
            comando.Parameters.AddWithValue("@dsRutaImagenRotada", pagina.DsRutaImagenRotada ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@dsRutaImagenRecortada", pagina.DsRutaImagenRecortada ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@nuGradosRotacion", pagina.NuGradosRotacion);
            comando.Parameters.AddWithValue("@snRotada", pagina.SnRotada);
            comando.Parameters.AddWithValue("@snRecortada", pagina.SnRecortada);
            comando.Parameters.AddWithValue("@nuAnchoOriginal", pagina.NuAnchoOriginal ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@nuAltoOriginal", pagina.NuAltoOriginal ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@cdEstado", pagina.CdEstado);
            comando.Parameters.AddWithValue("@feCreacion", pagina.FeCreacion);
            comando.Parameters.AddWithValue("@dsObservaciones", pagina.DsObservaciones ?? (object)DBNull.Value);
        }
    }
}
