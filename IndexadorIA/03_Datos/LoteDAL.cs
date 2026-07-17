using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Clase de acceso a datos para lotes
    /// </summary>
    public class LoteDAL
    {
        /// <summary>
        /// Obtiene todos los lotes de un proyecto
        /// </summary>
        public List<Lote> ObtenerPorProyecto(int cdProyecto)
        {
            List<Lote> lotes = new List<Lote>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdLote, cdProyecto, dsNombreLote, nuCantidadPaginas, cdEstado,
                                feCreacion, cdUsuarioCreacion, feEnvioBatch, feFinalizacion,
                                dsBatchId, dsResultado
                                FROM TD_LOTES 
                                WHERE cdProyecto = @cdProyecto
                                ORDER BY feCreacion DESC";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lotes.Add(MapearLote(reader));
                        }
                    }
                }
            }

            return lotes;
        }

        /// <summary>
        /// Obtiene todos los lotes
        /// </summary>
        public List<Lote> ObtenerTodos()
        {
            List<Lote> lotes = new List<Lote>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdLote, cdProyecto, dsNombreLote, nuCantidadPaginas, cdEstado,
                                feCreacion, cdUsuarioCreacion, feEnvioBatch, feFinalizacion,
                                dsBatchId, dsResultado
                                FROM TD_LOTES 
                                ORDER BY feCreacion DESC";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lotes.Add(MapearLote(reader));
                        }
                    }
                }
            }

            return lotes;
        }

        /// <summary>
        /// Obtiene un lote por su código
        /// </summary>
        public Lote? ObtenerPorCodigo(int cdLote)
        {
            Lote? lote = null;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdLote, cdProyecto, dsNombreLote, nuCantidadPaginas, cdEstado,
                                feCreacion, cdUsuarioCreacion, feEnvioBatch, feFinalizacion,
                                dsBatchId, dsResultado
                                FROM TD_LOTES 
                                WHERE cdLote = @cdLote";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdLote", cdLote);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lote = MapearLote(reader);
                        }
                    }
                }
            }

            return lote;
        }

        /// <summary>
        /// Obtiene lotes por estado
        /// </summary>
        public List<Lote> ObtenerPorEstado(int cdEstado)
        {
            List<Lote> lotes = new List<Lote>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT cdLote, cdProyecto, dsNombreLote, nuCantidadPaginas, cdEstado,
                                feCreacion, cdUsuarioCreacion, feEnvioBatch, feFinalizacion,
                                dsBatchId, dsResultado
                                FROM TD_LOTES 
                                WHERE cdEstado = @cdEstado
                                ORDER BY feCreacion DESC";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdEstado", cdEstado);
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lotes.Add(MapearLote(reader));
                        }
                    }
                }
            }

            return lotes;
        }

        /// <summary>
        /// Inserta un nuevo lote
        /// </summary>
        public int Insertar(Lote lote)
        {
            int nuevoId = 0;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"INSERT INTO TD_LOTES 
                                (cdProyecto, dsNombreLote, nuCantidadPaginas, cdEstado, feCreacion, cdUsuarioCreacion)
                                VALUES 
                                (@cdProyecto, @dsNombreLote, @nuCantidadPaginas, @cdEstado, @feCreacion, @cdUsuarioCreacion);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    AgregarParametrosBase(comando, lote);
                    conexion.Open();
                    nuevoId = (int)comando.ExecuteScalar();
                }
            }

            return nuevoId;
        }

        /// <summary>
        /// Crea un lote con sus páginas en una transacción
        /// </summary>
        public int CrearLoteConPaginas(Lote lote, List<int> cdPaginas)
        {
            int nuevoId = 0;

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // Insertar el lote
                        string queryLote = @"INSERT INTO TD_LOTES 
                                            (cdProyecto, dsNombreLote, nuCantidadPaginas, cdEstado, feCreacion, cdUsuarioCreacion)
                                            VALUES 
                                            (@cdProyecto, @dsNombreLote, @nuCantidadPaginas, @cdEstado, @feCreacion, @cdUsuarioCreacion);
                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        using (SqlCommand cmdLote = new SqlCommand(queryLote, conexion, transaccion))
                        {
                            lote.NuCantidadPaginas = cdPaginas.Count;
                            AgregarParametrosBase(cmdLote, lote);
                            nuevoId = (int)cmdLote.ExecuteScalar();
                        }

                        // Insertar las relaciones lote-páginas
                        string queryRelacion = @"INSERT INTO TR_LOTE_PAGINAS (cdLote, cdPagina, nuOrden, feAsignacion)
                                                VALUES (@cdLote, @cdPagina, @nuOrden, GETDATE())";

                        for (int i = 0; i < cdPaginas.Count; i++)
                        {
                            using (SqlCommand cmdRelacion = new SqlCommand(queryRelacion, conexion, transaccion))
                            {
                                cmdRelacion.Parameters.AddWithValue("@cdLote", nuevoId);
                                cmdRelacion.Parameters.AddWithValue("@cdPagina", cdPaginas[i]);
                                cmdRelacion.Parameters.AddWithValue("@nuOrden", i + 1);
                                cmdRelacion.ExecuteNonQuery();
                            }
                        }

                        // Actualizar estado de las páginas a "En Lote" (4)
                        string queryActualizar = "UPDATE TD_PAGINAS SET cdEstado = 4 WHERE cdPagina = @cdPagina";
                        foreach (int cdPagina in cdPaginas)
                        {
                            using (SqlCommand cmdActualizar = new SqlCommand(queryActualizar, conexion, transaccion))
                            {
                                cmdActualizar.Parameters.AddWithValue("@cdPagina", cdPagina);
                                cmdActualizar.ExecuteNonQuery();
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

            return nuevoId;
        }

        /// <summary>
        /// Actualiza un lote existente
        /// </summary>
        public bool Actualizar(Lote lote)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_LOTES SET 
                                dsNombreLote = @dsNombreLote,
                                nuCantidadPaginas = @nuCantidadPaginas,
                                cdEstado = @cdEstado,
                                feEnvioBatch = @feEnvioBatch,
                                feFinalizacion = @feFinalizacion,
                                dsBatchId = @dsBatchId,
                                dsResultado = @dsResultado
                                WHERE cdLote = @cdLote";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdLote", lote.CdLote);
                    comando.Parameters.AddWithValue("@dsNombreLote", lote.DsNombreLote);
                    comando.Parameters.AddWithValue("@nuCantidadPaginas", lote.NuCantidadPaginas);
                    comando.Parameters.AddWithValue("@cdEstado", lote.CdEstado);
                    comando.Parameters.AddWithValue("@feEnvioBatch", lote.FeEnvioBatch ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@feFinalizacion", lote.FeFinalizacion ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@dsBatchId", lote.DsBatchId ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@dsResultado", lote.DsResultado ?? (object)DBNull.Value);

                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Cambia el estado de un lote
        /// </summary>
        public bool CambiarEstado(int cdLote, int nuevoEstado)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_LOTES SET 
                                cdEstado = @cdEstado,
                                feEnvioBatch = CASE WHEN @cdEstado = 2 AND feEnvioBatch IS NULL THEN GETDATE() ELSE feEnvioBatch END,
                                feFinalizacion = CASE WHEN @cdEstado IN (4, 9) THEN GETDATE() ELSE feFinalizacion END
                                WHERE cdLote = @cdLote";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdLote", cdLote);
                    comando.Parameters.AddWithValue("@cdEstado", nuevoEstado);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Registra el Batch ID de OpenAI
        /// </summary>
        public bool RegistrarBatchId(int cdLote, string batchId)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_LOTES SET 
                                dsBatchId = @dsBatchId,
                                feEnvioBatch = GETDATE(),
                                cdEstado = 2
                                WHERE cdLote = @cdLote";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdLote", cdLote);
                    comando.Parameters.AddWithValue("@dsBatchId", batchId);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Guarda el resultado del procesamiento
        /// </summary>
        public bool GuardarResultado(int cdLote, string resultado)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"UPDATE TD_LOTES SET 
                                dsResultado = @dsResultado,
                                feFinalizacion = GETDATE(),
                                cdEstado = 4
                                WHERE cdLote = @cdLote";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdLote", cdLote);
                    comando.Parameters.AddWithValue("@dsResultado", resultado);
                    conexion.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Obtiene las páginas de un lote
        /// </summary>
        public List<Pagina> ObtenerPaginas(int cdLote)
        {
            List<Pagina> paginas = new List<Pagina>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT p.cdPagina, p.cdArchivo, p.nuNumeroPagina, p.dsRutaImagenOriginal,
                                p.dsRutaImagenRotada, p.dsRutaImagenRecortada, p.nuGradosRotacion, p.snRotada,
                                p.snRecortada, p.nuAnchoOriginal, p.nuAltoOriginal, p.cdEstado, p.feCreacion,
                                p.feProcesamiento, p.dsObservaciones
                                FROM TD_PAGINAS p
                                INNER JOIN TR_LOTE_PAGINAS lp ON p.cdPagina = lp.cdPagina
                                WHERE lp.cdLote = @cdLote
                                ORDER BY lp.nuOrden";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@cdLote", cdLote);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paginas.Add(new Pagina
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
                            });
                        }
                    }
                }
            }

            return paginas;
        }

        /// <summary>
        /// Elimina un lote y sus relaciones
        /// </summary>
        public bool Eliminar(int cdLote)
        {
            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                conexion.Open();
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // Actualizar páginas a estado Recortada (3)
                        string queryPaginas = @"UPDATE p SET p.cdEstado = 3
                                               FROM TD_PAGINAS p
                                               INNER JOIN TR_LOTE_PAGINAS lp ON p.cdPagina = lp.cdPagina
                                               WHERE lp.cdLote = @cdLote";

                        using (SqlCommand cmdPaginas = new SqlCommand(queryPaginas, conexion, transaccion))
                        {
                            cmdPaginas.Parameters.AddWithValue("@cdLote", cdLote);
                            cmdPaginas.ExecuteNonQuery();
                        }

                        // Eliminar relaciones
                        string queryRelaciones = "DELETE FROM TR_LOTE_PAGINAS WHERE cdLote = @cdLote";
                        using (SqlCommand cmdRelaciones = new SqlCommand(queryRelaciones, conexion, transaccion))
                        {
                            cmdRelaciones.Parameters.AddWithValue("@cdLote", cdLote);
                            cmdRelaciones.ExecuteNonQuery();
                        }

                        // Eliminar lote
                        string queryLote = "DELETE FROM TD_LOTES WHERE cdLote = @cdLote";
                        using (SqlCommand cmdLote = new SqlCommand(queryLote, conexion, transaccion))
                        {
                            cmdLote.Parameters.AddWithValue("@cdLote", cdLote);
                            cmdLote.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                        return true;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene estadísticas de lotes por proyecto
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticas(int cdProyecto)
        {
            var estadisticas = new Dictionary<string, int>();

            using (SqlConnection conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                string query = @"SELECT 
                                COUNT(*) as Total,
                                SUM(CASE WHEN cdEstado = 1 THEN 1 ELSE 0 END) as Pendientes,
                                SUM(CASE WHEN cdEstado = 2 THEN 1 ELSE 0 END) as Enviados,
                                SUM(CASE WHEN cdEstado = 3 THEN 1 ELSE 0 END) as Procesando,
                                SUM(CASE WHEN cdEstado = 4 THEN 1 ELSE 0 END) as Completados,
                                SUM(CASE WHEN cdEstado = 9 THEN 1 ELSE 0 END) as Errores,
                                ISNULL(SUM(nuCantidadPaginas), 0) as TotalPaginas
                                FROM TD_LOTES 
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
                            estadisticas["Enviados"] = reader.GetInt32(2);
                            estadisticas["Procesando"] = reader.GetInt32(3);
                            estadisticas["Completados"] = reader.GetInt32(4);
                            estadisticas["Errores"] = reader.GetInt32(5);
                            estadisticas["TotalPaginas"] = reader.GetInt32(6);
                        }
                    }
                }
            }

            return estadisticas;
        }

        /// <summary>
        /// Mapea un SqlDataReader a un objeto Lote
        /// </summary>
        private Lote MapearLote(SqlDataReader reader)
        {
            return new Lote
            {
                CdLote = reader.GetInt32(0),
                CdProyecto = reader.GetInt32(1),
                DsNombreLote = reader.GetString(2),
                NuCantidadPaginas = reader.GetInt32(3),
                CdEstado = reader.GetInt32(4),
                FeCreacion = reader.GetDateTime(5),
                CdUsuarioCreacion = reader.GetInt32(6),
                FeEnvioBatch = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                FeFinalizacion = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                DsBatchId = reader.IsDBNull(9) ? null : reader.GetString(9),
                DsResultado = reader.IsDBNull(10) ? null : reader.GetString(10)
            };
        }

        /// <summary>
        /// Agrega parámetros base a un comando
        /// </summary>
        private void AgregarParametrosBase(SqlCommand comando, Lote lote)
        {
            comando.Parameters.AddWithValue("@cdProyecto", lote.CdProyecto);
            comando.Parameters.AddWithValue("@dsNombreLote", lote.DsNombreLote);
            comando.Parameters.AddWithValue("@nuCantidadPaginas", lote.NuCantidadPaginas);
            comando.Parameters.AddWithValue("@cdEstado", lote.CdEstado);
            comando.Parameters.AddWithValue("@feCreacion", lote.FeCreacion);
            comando.Parameters.AddWithValue("@cdUsuarioCreacion", lote.CdUsuarioCreacion);
        }
    }
}
