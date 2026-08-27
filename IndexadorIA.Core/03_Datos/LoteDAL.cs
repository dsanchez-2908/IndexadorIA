using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Clase de acceso a datos para lotes de archivos página
    /// </summary>
    public class LoteDAL
    {
        private readonly string _cadenaConexion;

        public LoteDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene el siguiente número de secuencia para nombre de lote
        /// </summary>
        public int ObtenerSiguienteSecuencia()
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand("SELECT NEXT VALUE FOR SEQ_LOTE", conexion);
                conexion.Open();
                var resultado = comando.ExecuteScalar();
                return Convert.ToInt32(resultado);
            }
        }

        /// <summary>
        /// Crea un nuevo lote con sus archivos página en una transacción
        /// </summary>
        public int CrearLote(string dsNombreLote, int nuCantidadArchivos, int cdEstadoLote, 
                            int cdUsuarioAltaLote, List<int> cdArchivosPagina)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar lote
                        var cmdLote = new SqlCommand(
                            @"INSERT INTO TD_LOTE (dsNombreLote, nuCantidadArchivos, cdEstadoLote, 
                                                   feAltaLote, cdUsuarioAltaLote)
                              OUTPUT INSERTED.cdLote
                              VALUES (@dsNombreLote, @nuCantidadArchivos, @cdEstadoLote, 
                                     GETDATE(), @cdUsuarioAltaLote)", conexion, transaccion);

                        cmdLote.Parameters.AddWithValue("@dsNombreLote", dsNombreLote);
                        cmdLote.Parameters.AddWithValue("@nuCantidadArchivos", nuCantidadArchivos);
                        cmdLote.Parameters.AddWithValue("@cdEstadoLote", cdEstadoLote);
                        cmdLote.Parameters.AddWithValue("@cdUsuarioAltaLote", cdUsuarioAltaLote);

                        var resultadoLote = cmdLote.ExecuteScalar();
                        int cdLote = Convert.ToInt32(resultadoLote);

                        // 2. Insertar relaciones lote-archivos
                        foreach (int cdArchivoPagina in cdArchivosPagina)
                        {
                            var cmdRelacion = new SqlCommand(
                                @"INSERT INTO TD_LOTE_ARCHIVOS (cdLote, cdArchivoPagina)
                                  VALUES (@cdLote, @cdArchivoPagina)", conexion, transaccion);

                            cmdRelacion.Parameters.AddWithValue("@cdLote", cdLote);
                            cmdRelacion.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);
                            cmdRelacion.ExecuteNonQuery();
                        }

                        // 3. Actualizar estado de las páginas a cdEstado=2
                        foreach (int cdArchivoPagina in cdArchivosPagina)
                        {
                            var cmdUpdate = new SqlCommand(
                                @"UPDATE TD_ARCHIVOS_PAGINAS 
                                  SET cdEstado = 2 
                                  WHERE cdArchivoPagina = @cdArchivoPagina", conexion, transaccion);

                            cmdUpdate.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                        return cdLote;
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
        /// Obtiene archivos página filtrados para preparación de lotes
        /// </summary>
        public List<ArchivoPaginaGridDto> ObtenerArchivosPaginaParaLote(int cdProyecto, int cdEstado,
            string? dsNombreArchivo = null, DateTime? feAltaDesde = null, DateTime? feAltaHasta = null,
            string? dsCarpeta = null)
        {
            var archivos = new List<ArchivoPaginaGridDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var query = @"
                    SELECT a.cdArchivoPagina,
                           a.feAlta,
                           a.cdEstado,
                           c.dsEstado,
                           a.dsNombreArchivoPagina,
                           a.nuPagina,
                           a.dsRutaCompleta,
                           a.cdArchivoOriginal,
                           b.cdProyecto,
                           d.dsProyecto,
                           b.dsNombreArchivo,
                           b.dsNombreUltimaCarpeta,
                           b.nuCantidadPaginas			
                    FROM TD_ARCHIVOS_PAGINAS a
                    LEFT JOIN TD_ARCHIVOS_ORIGINAL b ON a.cdArchivoOriginal = b.cdArchivo
                    LEFT JOIN TD_ESTADOS c ON c.dsProceso = 'ARCHIVO_PAGINA' AND a.cdEstado = c.cdEstado
                    LEFT JOIN TD_PROYECTOS d ON b.cdProyecto = d.cdProyecto
                    WHERE b.cdProyecto = @cdProyecto AND a.cdEstado = @cdEstado";

                var comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);
                comando.Parameters.AddWithValue("@cdEstado", cdEstado);

                // Filtros opcionales
                if (!string.IsNullOrEmpty(dsNombreArchivo))
                {
                    comando.CommandText += " AND b.dsNombreArchivo LIKE @dsNombreArchivo";
                    comando.Parameters.AddWithValue("@dsNombreArchivo", $"%{dsNombreArchivo}%");
                }

                if (feAltaDesde.HasValue)
                {
                    comando.CommandText += " AND a.feAlta >= @feAltaDesde";
                    comando.Parameters.AddWithValue("@feAltaDesde", feAltaDesde.Value);
                }

                if (feAltaHasta.HasValue)
                {
                    comando.CommandText += " AND a.feAlta <= @feAltaHasta";
                    comando.Parameters.AddWithValue("@feAltaHasta", feAltaHasta.Value.AddDays(1).AddSeconds(-1));
                }

                if (!string.IsNullOrEmpty(dsCarpeta))
                {
                    comando.CommandText += " AND b.dsNombreUltimaCarpeta LIKE @dsCarpeta";
                    comando.Parameters.AddWithValue("@dsCarpeta", $"%{dsCarpeta}%");
                }

                comando.CommandText += " ORDER BY a.feAlta, a.cdArchivoPagina";

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        archivos.Add(new ArchivoPaginaGridDto
                        {
                            CdArchivoPagina = lector.GetInt32(0),
                            FeAlta = lector.GetDateTime(1),
                            CdEstado = lector.GetInt32(2),
                            DsEstado = lector.IsDBNull(3) ? "" : lector.GetString(3),
                            DsNombreArchivoPagina = lector.GetString(4),
                            NuPagina = lector.GetInt32(5),
                            DsRutaCompleta = lector.GetString(6),
                            CdArchivoOriginal = lector.GetInt32(7),
                            CdProyecto = lector.IsDBNull(8) ? 0 : lector.GetInt32(8),
                            DsProyecto = lector.IsDBNull(9) ? "" : lector.GetString(9),
                            DsNombreArchivo = lector.IsDBNull(10) ? "" : lector.GetString(10),
                            DsNombreUltimaCarpeta = lector.IsDBNull(11) ? "" : lector.GetString(11),
                            NuCantidadPaginas = lector.IsDBNull(12) ? 0 : lector.GetInt32(12)
                        });
                    }
                }
            }

            return archivos;
        }

        /// <summary>
        /// Actualiza el estado de un lote
        /// </summary>
        public void ActualizarEstado(int cdLote, int cdEstado, int cdUsuario)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    UPDATE TD_LOTE 
                    SET cdEstadoLote = @cdEstado
                    WHERE cdLote = @cdLote", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);
                comando.Parameters.AddWithValue("@cdEstado", cdEstado);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Obtiene los archivos/páginas de un lote con sus rutas Base64
        /// </summary>
        public List<ArchivoPagina> ObtenerArchivosPaginasPorLote(int cdLote)
        {
            var archivos = new List<ArchivoPagina>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    SELECT ap.cdArchivoPagina, ap.dsNombreArchivoPagina, ap.nuPagina, 
                           ap.dsRutaCompleta, a.dsNombreArchivo,
                           REPLACE(REPLACE(ap.dsRutaCompleta, '.png', '.b64'), '.pdf', '.b64') as RutaBase64
                    FROM TD_LOTE_ARCHIVOS la
                    INNER JOIN TD_ARCHIVOS_PAGINAS ap ON la.cdArchivoPagina = ap.cdArchivoPagina
                    INNER JOIN TD_ARCHIVOS_ORIGINAL a ON ap.cdArchivoOriginal = a.cdArchivo
                    WHERE la.cdLote = @cdLote
                    ORDER BY a.dsNombreArchivo, ap.nuPagina", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        archivos.Add(new ArchivoPagina
                        {
                            CdArchivoPagina = lector.GetInt32(0),
                            DsNombreArchivoPagina = lector.GetString(1),
                            NuPagina = lector.GetInt32(2),
                            DsRutaCompleta = lector.GetString(3),
                            NombreArchivo = lector.GetString(4),
                            RutaBase64 = lector.GetString(5)
                        });
                    }
                }
            }

            return archivos;
        }

        /// <summary>
        /// Obtiene la fecha de finalización del procesamiento IA (batch) de un lote,
        /// o null si el lote aún no fue procesado / no tiene batch completado.
        /// </summary>
        public DateTime? ObtenerFechaProcesamientoIA(int cdLote)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    SELECT MAX(feCompleted)
                    FROM TD_BATCH_TRACKING
                    WHERE cdLote = @cdLote", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);

                conexion.Open();
                var resultado = comando.ExecuteScalar();

                return (resultado == null || resultado == DBNull.Value)
                    ? (DateTime?)null
                    : Convert.ToDateTime(resultado);
            }
        }

        /// <summary>
        /// Obtiene un lote por su identificador.
        /// </summary>
        public Lote? ObtenerPorId(int cdLote)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    SELECT l.cdLote, l.dsNombreLote, l.nuCantidadArchivos, l.cdEstadoLote,
                           l.feAltaLote, l.cdUsuarioAltaLote, e.dsEstado
                    FROM TD_LOTE l
                    LEFT JOIN TD_ESTADOS e ON e.dsProceso = 'LOTE' AND e.cdEstado = l.cdEstadoLote
                    WHERE l.cdLote = @cdLote", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return new Lote
                        {
                            CdLote = lector.GetInt32(0),
                            DsNombreLote = lector.GetString(1),
                            NuCantidadArchivos = lector.GetInt32(2),
                            CdEstadoLote = lector.GetInt32(3),
                            FeAltaLote = lector.GetDateTime(4),
                            CdUsuarioAltaLote = lector.IsDBNull(5) ? null : lector.GetInt32(5),
                            DsEstado = lector.IsDBNull(6) ? null : lector.GetString(6)
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Obtiene los lotes con un estado específico para procesamiento
        /// </summary>
        public List<Lote> ObtenerLotesPorEstado(int cdEstado, int? cdProyecto = null)
        {
            var lotes = new List<Lote>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var sql = @"
                    SELECT l.cdLote, l.dsNombreLote, l.nuCantidadArchivos, 
                           l.cdEstadoLote, e.dsEstado, l.feAltaLote
                    FROM TD_LOTE l
                    INNER JOIN TD_ESTADOS e ON e.dsProceso = 'LOTE' AND e.cdEstado = l.cdEstadoLote
                    WHERE l.cdEstadoLote = @cdEstado";

                if (cdProyecto.HasValue)
                {
                    sql += " AND EXISTS (SELECT 1 FROM TD_LOTE_ARCHIVOS la " +
                           "INNER JOIN TD_ARCHIVOS_PAGINAS ap ON la.cdArchivoPagina = ap.cdArchivoPagina " +
                           "INNER JOIN TD_ARCHIVOS_ORIGINAL a ON ap.cdArchivoOriginal = a.cdArchivo " +
                           "WHERE la.cdLote = l.cdLote AND a.cdProyecto = @cdProyecto)";
                }

                sql += " ORDER BY l.feAltaLote DESC";

                var comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@cdEstado", cdEstado);
                if (cdProyecto.HasValue)
                    comando.Parameters.AddWithValue("@cdProyecto", cdProyecto.Value);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lotes.Add(new Lote
                        {
                            CdLote = lector.GetInt32(0),
                            DsNombreLote = lector.GetString(1),
                            NuCantidadArchivos = lector.GetInt32(2),
                            CdEstadoLote = lector.GetInt32(3),
                            DsEstado = lector.GetString(4),
                            FeAltaLote = lector.GetDateTime(5)
                        });
                    }
                }
            }

            return lotes;
        }

        /// <summary>
        /// Obtiene lotes filtrados por estado, nombre de lote, rango de fecha de alta
        /// y opcionalmente por usuario asignado
        /// </summary>
        public List<Lote> ObtenerLotesPorEstadoFiltrado(int cdEstado, string? dsNombreLote = null,
            DateTime? feAltaDesde = null, DateTime? feAltaHasta = null, int? cdUsuarioAsignado = null)
        {
            var lotes = new List<Lote>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var sql = @"
                    SELECT l.cdLote, l.dsNombreLote, l.nuCantidadArchivos, 
                           l.cdEstadoLote, e.dsEstado, l.feAltaLote, l.cdUsuarioAsignado,
                           ISNULL((SELECT COUNT(*) FROM TD_001_RESULTADO_IA r WHERE r.cdLote = l.cdLote), 0) AS nuCorrectos,
                           ISNULL((SELECT COUNT(*) FROM TD_001_RESULTADO_IA_ERROR er WHERE er.cdLote = l.cdLote), 0) AS nuIncorrectos
                    FROM TD_LOTE l
                    INNER JOIN TD_ESTADOS e ON e.dsProceso = 'LOTE' AND e.cdEstado = l.cdEstadoLote
                    WHERE l.cdEstadoLote = @cdEstado";

                if (!string.IsNullOrEmpty(dsNombreLote))
                {
                    sql += " AND l.dsNombreLote LIKE @dsNombreLote";
                }

                if (feAltaDesde.HasValue)
                {
                    sql += " AND l.feAltaLote >= @feAltaDesde";
                }

                if (feAltaHasta.HasValue)
                {
                    sql += " AND l.feAltaLote < @feAltaHasta";
                }

                if (cdUsuarioAsignado.HasValue)
                {
                    sql += " AND l.cdUsuarioAsignado = @cdUsuarioAsignado";
                }

                sql += " ORDER BY l.feAltaLote DESC";

                var comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@cdEstado", cdEstado);

                if (!string.IsNullOrEmpty(dsNombreLote))
                    comando.Parameters.AddWithValue("@dsNombreLote", $"%{dsNombreLote}%");

                if (feAltaDesde.HasValue)
                    comando.Parameters.AddWithValue("@feAltaDesde", feAltaDesde.Value.Date);

                if (feAltaHasta.HasValue)
                    comando.Parameters.AddWithValue("@feAltaHasta", feAltaHasta.Value.Date.AddDays(1));

                if (cdUsuarioAsignado.HasValue)
                    comando.Parameters.AddWithValue("@cdUsuarioAsignado", cdUsuarioAsignado.Value);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lotes.Add(new Lote
                        {
                            CdLote = lector.GetInt32(0),
                            DsNombreLote = lector.GetString(1),
                            NuCantidadArchivos = lector.GetInt32(2),
                            CdEstadoLote = lector.GetInt32(3),
                            DsEstado = lector.GetString(4),
                            FeAltaLote = lector.GetDateTime(5),
                            CdUsuarioAsignado = lector.IsDBNull(6) ? null : lector.GetInt32(6),
                            NuCorrectos = lector.GetInt32(7),
                            NuIncorrectos = lector.GetInt32(8)
                        });
                    }
                }
            }

            return lotes;
        }

        /// <summary>
        /// Asigna una lista de lotes a un usuario para su control, cambiando su estado a "Controlando" (cdEstado=6)
        /// </summary>
        public void AsignarLotes(List<int> cdLotes, int cdUsuarioAsignado)
        {
            const int CD_ESTADO_CONTROLANDO = 6;

            if (cdLotes == null || cdLotes.Count == 0)
            {
                return;
            }

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        foreach (int cdLote in cdLotes)
                        {
                            var comando = new SqlCommand(@"
                                UPDATE TD_LOTE
                                SET cdEstadoLote = @cdEstadoLote,
                                    cdUsuarioAsignado = @cdUsuarioAsignado
                                WHERE cdLote = @cdLote", conexion, transaccion);

                            comando.Parameters.AddWithValue("@cdEstadoLote", CD_ESTADO_CONTROLANDO);
                            comando.Parameters.AddWithValue("@cdUsuarioAsignado", cdUsuarioAsignado);
                            comando.Parameters.AddWithValue("@cdLote", cdLote);

                            comando.ExecuteNonQuery();
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
        }

        /// <summary>
        /// Obtiene los lotes en un estado especifico (por defecto 7 - Pendiente de Finalizar)
        /// junto con estadisticas de estado de control y cantidad de correcciones manuales,
        /// aplicando filtros opcionales de nombre de lote y rango de fecha de control.
        /// </summary>
        public List<Entidades.LoteFinalizacionGridDto> ObtenerLotesPendientesFinalizarConEstadisticas(
            string? dsNombreLote = null, DateTime? feControlDesde = null, DateTime? feControlHasta = null,
            int cdEstadoLote = 7)
        {
            var lotes = new List<Entidades.LoteFinalizacionGridDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var sql = @"
                    SELECT l.cdLote, l.dsNombreLote, u.dsNombreCompleto,
                           COUNT(r.cdResultado) AS nuCantidadPlanos,
                           SUM(CASE WHEN r.cdEstadoControl = 2 THEN 1 ELSE 0 END) AS nuCantidadControlado,
                           SUM(CASE WHEN r.cdEstadoControl = 1 THEN 1 ELSE 0 END) AS nuCantidadPendiente,
                           SUM(CASE WHEN r.cdEstadoControl = 3 THEN 1 ELSE 0 END) AS nuCantidadPaginaIlegible,
                           SUM(CASE WHEN r.cdEstadoControl = 4 THEN 1 ELSE 0 END) AS nuCantidadDatosIlegibles,
                           (SELECT COUNT(DISTINCT c.cdResultado)
                            FROM TD_CORRECIONES c
                            INNER JOIN TD_001_RESULTADO_IA ri ON ri.cdResultado = c.cdResultado
                            WHERE ri.cdLote = l.cdLote) AS nuCantidadCorregido
                    FROM TD_LOTE l
                    LEFT JOIN TD_USUARIOS u ON u.cdUsuario = l.cdUsuarioAsignado
                    LEFT JOIN TD_001_RESULTADO_IA r ON r.cdLote = l.cdLote
                    WHERE l.cdEstadoLote = @cdEstadoLote";

                if (!string.IsNullOrEmpty(dsNombreLote))
                {
                    sql += " AND l.dsNombreLote LIKE @dsNombreLote";
                }

                if (feControlDesde.HasValue)
                {
                    sql += " AND EXISTS (SELECT 1 FROM TD_001_RESULTADO_IA rf WHERE rf.cdLote = l.cdLote AND rf.feControl >= @feControlDesde)";
                }

                if (feControlHasta.HasValue)
                {
                    sql += " AND EXISTS (SELECT 1 FROM TD_001_RESULTADO_IA rf WHERE rf.cdLote = l.cdLote AND rf.feControl < @feControlHasta)";
                }

                sql += @"
                    GROUP BY l.cdLote, l.dsNombreLote, u.dsNombreCompleto
                    ORDER BY l.cdLote DESC";

                var comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@cdEstadoLote", cdEstadoLote);

                if (!string.IsNullOrEmpty(dsNombreLote))
                    comando.Parameters.AddWithValue("@dsNombreLote", $"%{dsNombreLote}%");

                if (feControlDesde.HasValue)
                    comando.Parameters.AddWithValue("@feControlDesde", feControlDesde.Value.Date);

                if (feControlHasta.HasValue)
                    comando.Parameters.AddWithValue("@feControlHasta", feControlHasta.Value.Date.AddDays(1));

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lotes.Add(new Entidades.LoteFinalizacionGridDto
                        {
                            CdLote = lector.GetInt32(0),
                            DsNombreLote = lector.GetString(1),
                            DsUsuarioAsignado = lector.IsDBNull(2) ? null : lector.GetString(2),
                            NuCantidadPlanos = lector.GetInt32(3),
                            NuCantidadControlado = lector.GetInt32(4),
                            NuCantidadPendiente = lector.GetInt32(5),
                            NuCantidadPaginaIlegible = lector.GetInt32(6),
                            NuCantidadDatosIlegibles = lector.GetInt32(7),
                            NuCantidadCorregido = lector.GetInt32(8)
                        });
                    }
                }
            }

            return lotes;
        }

        /// <summary>
        /// Obtiene, agrupado por usuario asignado, el resumen de lotes y planos
        /// asignados/controlados/pendientes para la pantalla Monitor de Lotes.
        /// Se consideran "asignados" todos los lotes que pasaron por el flujo de control
        /// (estados 6 Controlando, 7 Pendiente de Finalizar y 3 Finalizado), "controlados"
        /// los que ya completaron el control (estados 7 y 3) y "pendientes" los que
        /// todavía están en control (estado 6).
        /// </summary>
        public List<Entidades.MonitorLoteUsuarioDto> ObtenerMonitorLotesPorUsuario()
        {
            var resultado = new List<Entidades.MonitorLoteUsuarioDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var sql = @"
                    SELECT u.cdUsuario, u.dsNombreCompleto,
                           COUNT(DISTINCT l.cdLote) AS nuTotalLotesAsignados,
                           ISNULL(SUM(l.nuCantidadArchivos), 0) AS nuCantidadPlanosAsignados,
                           COUNT(DISTINCT CASE WHEN l.cdEstadoLote IN (3, 7) THEN l.cdLote END) AS nuTotalLotesControlados,
                           ISNULL(SUM(CASE WHEN l.cdEstadoLote IN (3, 7) THEN l.nuCantidadArchivos ELSE 0 END), 0) AS nuCantidadPlanosControlados,
                           COUNT(DISTINCT CASE WHEN l.cdEstadoLote = 6 THEN l.cdLote END) AS nuTotalLotesPendientes,
                           ISNULL(SUM(CASE WHEN l.cdEstadoLote = 6 THEN l.nuCantidadArchivos ELSE 0 END), 0) AS nuCantidadPlanosPendientes
                    FROM TD_LOTE l
                    INNER JOIN TD_USUARIOS u ON u.cdUsuario = l.cdUsuarioAsignado
                    WHERE l.cdEstadoLote IN (6, 7, 3)
                    GROUP BY u.cdUsuario, u.dsNombreCompleto
                    ORDER BY u.dsNombreCompleto";

                var comando = new SqlCommand(sql, conexion);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        resultado.Add(new Entidades.MonitorLoteUsuarioDto
                        {
                            CdUsuario = lector.GetInt32(0),
                            DsUsuario = lector.GetString(1),
                            NuTotalLotesAsignados = lector.GetInt32(2),
                            NuCantidadPlanosAsignados = lector.GetInt32(3),
                            NuTotalLotesControlados = lector.GetInt32(4),
                            NuCantidadPlanosControlados = lector.GetInt32(5),
                            NuTotalLotesPendientes = lector.GetInt32(6),
                            NuCantidadPlanosPendientes = lector.GetInt32(7)
                        });
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el detalle de lotes de un usuario asignado, con estadísticas de control
        /// por página y filtro opcional de estado: null/0 = Todo, 6 = Pendiente de Control,
        /// -1 = Controlado (incluye Pendiente de Finalizar (7) y Finalizado (3)).
        /// </summary>
        public List<Entidades.LoteMonitorDetalleDto> ObtenerLotesPorUsuarioConEstadisticas(
            int cdUsuarioAsignado, int? filtroEstado = null)
        {
            var lotes = new List<Entidades.LoteMonitorDetalleDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var sql = @"
                    SELECT l.cdLote, l.dsNombreLote, l.cdEstadoLote, e.dsEstado,
                           COUNT(r.cdResultado) AS nuCantidadPlanos,
                           SUM(CASE WHEN r.cdEstadoControl = 2 THEN 1 ELSE 0 END) AS nuCantidadControlado,
                           SUM(CASE WHEN r.cdEstadoControl = 1 THEN 1 ELSE 0 END) AS nuCantidadPendiente,
                           SUM(CASE WHEN r.cdEstadoControl = 3 THEN 1 ELSE 0 END) AS nuCantidadPaginaIlegible,
                           SUM(CASE WHEN r.cdEstadoControl = 4 THEN 1 ELSE 0 END) AS nuCantidadDatosIlegibles,
                           (SELECT COUNT(DISTINCT c.cdResultado)
                            FROM TD_CORRECIONES c
                            INNER JOIN TD_001_RESULTADO_IA ri ON ri.cdResultado = c.cdResultado
                            WHERE ri.cdLote = l.cdLote) AS nuCantidadCorregido
                    FROM TD_LOTE l
                    LEFT JOIN TD_ESTADOS e ON e.dsProceso = 'LOTE' AND e.cdEstado = l.cdEstadoLote
                    LEFT JOIN TD_001_RESULTADO_IA r ON r.cdLote = l.cdLote
                    WHERE l.cdUsuarioAsignado = @cdUsuarioAsignado
                      AND l.cdEstadoLote IN (6, 7, 3)";

                if (filtroEstado.HasValue && filtroEstado.Value == 6)
                {
                    sql += " AND l.cdEstadoLote = 6";
                }
                else if (filtroEstado.HasValue && filtroEstado.Value == -1)
                {
                    sql += " AND l.cdEstadoLote IN (7, 3)";
                }

                sql += @"
                    GROUP BY l.cdLote, l.dsNombreLote, l.cdEstadoLote, e.dsEstado
                    ORDER BY l.cdLote DESC";

                var comando = new SqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@cdUsuarioAsignado", cdUsuarioAsignado);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lotes.Add(new Entidades.LoteMonitorDetalleDto
                        {
                            CdLote = lector.GetInt32(0),
                            DsNombreLote = lector.GetString(1),
                            CdEstadoLote = lector.GetInt32(2),
                            DsEstadoLote = lector.IsDBNull(3) ? string.Empty : lector.GetString(3),
                            NuCantidadPlanos = lector.GetInt32(4),
                            NuCantidadControlado = lector.GetInt32(5),
                            NuCantidadPendiente = lector.GetInt32(6),
                            NuCantidadPaginaIlegible = lector.GetInt32(7),
                            NuCantidadDatosIlegibles = lector.GetInt32(8),
                            NuCantidadCorregido = lector.GetInt32(9)
                        });
                    }
                }
            }

            return lotes;
        }

        /// <summary>
        /// Actualiza el nombre de archivo final (ya renombrado/movido) de una pagina.
        /// </summary>
        public void ActualizarNombreArchivoFinal(int cdArchivoPagina, string dsNombreArchivoFinal)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    UPDATE TD_ARCHIVOS_PAGINAS
                    SET dsNombreArchivoFinal = @dsNombreArchivoFinal
                    WHERE cdArchivoPagina = @cdArchivoPagina", conexion);

                comando.Parameters.AddWithValue("@dsNombreArchivoFinal", dsNombreArchivoFinal);
                comando.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Agrega un texto a las observaciones existentes del resultado IA asociado a la
        /// pagina indicada (usado, por ejemplo, para marcar "NOMBRE DE ARCHIVO INCOMPLETO"
        /// cuando el nombre final del PDF debio truncarse por exceder los 260 caracteres).
        /// </summary>
        public void AgregarObservacion(int cdArchivoPagina, string textoAAgregar)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    UPDATE TD_001_RESULTADO_IA
                    SET dsObservaciones = CASE
                        WHEN dsObservaciones IS NULL OR dsObservaciones = '' THEN @texto
                        ELSE dsObservaciones + ' ' + @texto
                    END
                    WHERE cdArchivoPagina = @cdArchivoPagina", conexion);

                comando.Parameters.AddWithValue("@texto", textoAAgregar);
                comando.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Obtiene las filas de metadatos de la vista VW_001_RESULTADO_IA para un lote,
        /// usadas para generar los CSV de finalizacion.
        /// </summary>
        public List<Entidades.ResultadoIAMetadataDto> ObtenerMetadatosParaCsv(int cdLote)
        {
            var filas = new List<Entidades.ResultadoIAMetadataDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    SELECT dsRutaCompleta, dsNombreArchivoOriginal, dsNombreArchivoFinal,
                           dsCategoriaPlano, dsTipoPlano, dsAcronimo, dsDireccion, dsSeccion,
                           dsManzana, dsParcela, dsExpediente, dsNumeroPlano, cdEstadoControl,
                           dsObservaciones
                    FROM VW_001_RESULTADO_IA
                    WHERE cdLote = @cdLote", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        filas.Add(new Entidades.ResultadoIAMetadataDto
                        {
                            DsRutaCompleta = lector.IsDBNull(0) ? string.Empty : lector.GetString(0),
                            DsNombreArchivoOriginal = lector.IsDBNull(1) ? string.Empty : lector.GetString(1),
                            DsNombreArchivoFinal = lector.IsDBNull(2) ? string.Empty : lector.GetString(2),
                            DsCategoriaPlano = lector.IsDBNull(3) ? string.Empty : lector.GetString(3),
                            DsTipoPlano = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
                            DsAcronimo = lector.IsDBNull(5) ? string.Empty : lector.GetString(5),
                            DsDireccion = lector.IsDBNull(6) ? string.Empty : lector.GetString(6),
                            DsSeccion = lector.IsDBNull(7) ? string.Empty : lector.GetString(7),
                            DsManzana = lector.IsDBNull(8) ? string.Empty : lector.GetString(8),
                            DsParcela = lector.IsDBNull(9) ? string.Empty : lector.GetString(9),
                            DsExpediente = lector.IsDBNull(10) ? string.Empty : lector.GetString(10),
                            DsNumeroPlano = lector.IsDBNull(11) ? string.Empty : lector.GetString(11),
                            CdEstadoControl = lector.GetInt32(12),
                            DsObservaciones = lector.IsDBNull(13) ? null : lector.GetString(13)
                        });
                    }
                }
            }

            return filas;
        }

        /// <summary>
        /// Obtiene el detalle de registros de un lote desde VW_001_RESULTADO_IA,
        /// para la pantalla de "Ver Lote" previa a la asignación.
        /// </summary>
        public List<Entidades.ResultadoIADetalleLoteDto> ObtenerDetalleLoteParaVer(int cdLote)
        {
            var filas = new List<Entidades.ResultadoIADetalleLoteDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    SELECT		a.cdLote,
                                b.dsNombreLote,
                                a.dsCategoriaPlano,
                                a.dsTipoPlano,
                                a.dsAcronimo,
                                a.dsDireccion,
                                a.dsSeccion,
                                a.dsManzana,
                                a.dsParcela,
                                a.dsExpediente,
                                a.dsNumeroPlano
                    FROM		VW_001_RESULTADO_IA	a
                    LEFT JOIN	TD_LOTE				b ON a.cdLote=b.cdLote
                    WHERE		a.cdLote = @cdLote", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        filas.Add(new Entidades.ResultadoIADetalleLoteDto
                        {
                            CdLote = lector.GetInt32(0),
                            DsNombreLote = lector.IsDBNull(1) ? null : lector.GetString(1),
                            DsCategoriaPlano = lector.IsDBNull(2) ? null : lector.GetString(2),
                            DsTipoPlano = lector.IsDBNull(3) ? null : lector.GetString(3),
                            DsAcronimo = lector.IsDBNull(4) ? null : lector.GetString(4),
                            DsDireccion = lector.IsDBNull(5) ? null : lector.GetString(5),
                            DsSeccion = lector.IsDBNull(6) ? null : lector.GetString(6),
                            DsManzana = lector.IsDBNull(7) ? null : lector.GetString(7),
                            DsParcela = lector.IsDBNull(8) ? null : lector.GetString(8),
                            DsExpediente = lector.IsDBNull(9) ? null : lector.GetString(9),
                            DsNumeroPlano = lector.IsDBNull(10) ? null : lector.GetString(10)
                        });
                    }
                }
            }

            return filas;
        }
    }
}
