using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;
using System.Linq;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_001_RESULTADO_IA
    /// </summary>
    public class ResultadoIADAL
    {
        private readonly string _cadenaConexion;

        public ResultadoIADAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Inserta un nuevo resultado de IA y retorna el ID generado.
        /// Si ya existiera un resultado previo para la misma página (por ejemplo, al reprocesar
        /// un batch), se elimina antes de insertar el nuevo para evitar duplicados.
        /// </summary>
        public int Insertar(ResultadoIA resultado)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                conn.Open();

                using (var tx = conn.BeginTransaction())
                {
                    // Eliminar primero los tokens asociados a resultados previos de esta página
                    // (evita conflicto de FK_TD_TOKEN_RESULTADO al borrar TD_001_RESULTADO_IA)
                    using (var cmdDeleteTokens = new SqlCommand(
                        @"DELETE FROM TD_TOKEN WHERE cdResultado IN
                          (SELECT cdResultado FROM TD_001_RESULTADO_IA WHERE cdArchivoPagina = @cdArchivoPagina)", conn, tx))
                    {
                        cmdDeleteTokens.Parameters.AddWithValue("@cdArchivoPagina", resultado.CdArchivoPagina);
                        cmdDeleteTokens.ExecuteNonQuery();
                    }

                    using (var cmdDelete = new SqlCommand(
                        "DELETE FROM TD_001_RESULTADO_IA WHERE cdArchivoPagina = @cdArchivoPagina", conn, tx))
                    {
                        cmdDelete.Parameters.AddWithValue("@cdArchivoPagina", resultado.CdArchivoPagina);
                        cmdDelete.ExecuteNonQuery();
                    }

                    tx.Commit();
                }

                using var cmd = new SqlCommand(@"
                    INSERT INTO TD_001_RESULTADO_IA 
                    (cdLote, cdArchivoPagina, cdCategoriaPlano, cdTipoPlano, dsExpediente, dsSeccion, dsManzana, dsParcela, dsDireccion, dsNumeroPlano,
                     nuConfianzaCategoriaPlano, nuConfianzaTipoPlano, nuConfianzaExpediente, nuConfianzaSeccion, nuConfianzaManzana, 
                     nuConfianzaParcela, nuConfianzaDireccion, nuConfianzaNumeroPlano, feAlta, cdUsuarioAlta)
                    VALUES 
                    (@cdLote, @cdArchivoPagina, @cdCategoriaPlano, @cdTipoPlano, @dsExpediente, @dsSeccion, @dsManzana, @dsParcela, @dsDireccion, @dsNumeroPlano,
                     @nuConfianzaCategoriaPlano, @nuConfianzaTipoPlano, @nuConfianzaExpediente, @nuConfianzaSeccion, @nuConfianzaManzana, 
                     @nuConfianzaParcela, @nuConfianzaDireccion, @nuConfianzaNumeroPlano, GETDATE(), @cdUsuarioAlta);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);", conn);

                cmd.Parameters.AddWithValue("@cdLote", resultado.CdLote);
                cmd.Parameters.AddWithValue("@cdArchivoPagina", resultado.CdArchivoPagina);
                cmd.Parameters.AddWithValue("@cdCategoriaPlano", (object?)resultado.CdCategoriaPlano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cdTipoPlano", (object?)resultado.CdTipoPlano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsExpediente", (object?)resultado.DsExpediente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsSeccion", (object?)resultado.DsSeccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsManzana", (object?)resultado.DsManzana ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsParcela", (object?)resultado.DsParcela ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsDireccion", (object?)resultado.DsDireccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsNumeroPlano", (object?)resultado.DsNumeroPlano ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@nuConfianzaCategoriaPlano", (object?)resultado.NuConfianzaCategoriaPlano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaTipoPlano", (object?)resultado.NuConfianzaTipoPlano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaExpediente", (object?)resultado.NuConfianzaExpediente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaSeccion", (object?)resultado.NuConfianzaSeccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaManzana", (object?)resultado.NuConfianzaManzana ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaParcela", (object?)resultado.NuConfianzaParcela ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaDireccion", (object?)resultado.NuConfianzaDireccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuConfianzaNumeroPlano", (object?)resultado.NuConfianzaNumeroPlano ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@cdUsuarioAlta", resultado.CdUsuarioAlta);

                int cdResultado = (int)cmd.ExecuteScalar()!;

                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "ResultadoIADAL.Insertar",
                    DsMensaje = $"Resultado IA insertado: {cdResultado} para archivo/página {resultado.CdArchivoPagina}"
                });

                return cdResultado;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "ResultadoIADAL.Insertar",
                    DsMensaje = $"Error al insertar resultado IA: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los resultados de un lote
        /// </summary>
        /// <param name="cdLote">Código del lote</param>
        /// <param name="mostrarTodos">Si es false (por defecto), solo trae los registros pendientes de control (cdEstadoControl = 1)</param>
        public List<ResultadoIA> ObtenerPorLote(int cdLote, bool mostrarTodos = false)
        {
            List<ResultadoIA> resultados = new();

            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand($@"
                    SELECT r.cdResultado, r.cdLote, r.cdArchivoPagina, r.cdCategoriaPlano, r.cdTipoPlano, 
                           r.dsExpediente, r.dsSeccion, r.dsManzana, r.dsParcela, r.dsDireccion, r.dsNumeroPlano,
                           r.nuConfianzaCategoriaPlano, r.nuConfianzaTipoPlano, r.nuConfianzaExpediente, r.nuConfianzaSeccion, 
                           r.nuConfianzaManzana, r.nuConfianzaParcela, r.nuConfianzaDireccion, r.nuConfianzaNumeroPlano,
                           r.feAlta, r.cdUsuarioAlta, r.feUltimaModificacion, r.cdUsuarioModificacion,
                           cp.dsCategoriaPlano, tp.dsTipoPlano, a.dsNombreArchivo, ap.nuPagina,
                           r.cdEstadoControl, r.snModificaDatos, r.feControl, r.cdUsuarioControl, e.dsEstado
                    FROM TD_001_RESULTADO_IA r
                    LEFT JOIN TD_CATEGORIA_PLANO cp ON r.cdCategoriaPlano = cp.cdCategoriaPlano
                    LEFT JOIN TD_TIPOS_PLANO tp ON r.cdTipoPlano = tp.cdTipoPlano
                    LEFT JOIN TD_ARCHIVOS_PAGINAS ap ON r.cdArchivoPagina = ap.cdArchivoPagina
                    LEFT JOIN TD_ARCHIVOS_ORIGINAL a ON ap.cdArchivoOriginal = a.cdArchivo
                    LEFT JOIN TD_ESTADOS e ON e.dsProceso = 'CONTROL' AND e.cdEstado = r.cdEstadoControl
                    INNER JOIN TD_LOTE_ARCHIVOS la ON la.cdArchivoPagina = r.cdArchivoPagina
                    WHERE la.cdLote = @cdLote
                    {(mostrarTodos ? "" : "AND r.cdEstadoControl = 1")}
                    ORDER BY a.dsNombreArchivo, ap.nuPagina", conn);

                cmd.Parameters.AddWithValue("@cdLote", cdLote);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    resultados.Add(new ResultadoIA
                    {
                        CdResultado = reader.GetInt32(0),
                        CdLote = reader.GetInt32(1),
                        CdArchivoPagina = reader.GetInt32(2),
                        CdCategoriaPlano = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        CdTipoPlano = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        DsExpediente = reader.IsDBNull(5) ? null : reader.GetString(5),
                        DsSeccion = reader.IsDBNull(6) ? null : reader.GetString(6),
                        DsManzana = reader.IsDBNull(7) ? null : reader.GetString(7),
                        DsParcela = reader.IsDBNull(8) ? null : reader.GetString(8),
                        DsDireccion = reader.IsDBNull(9) ? null : reader.GetString(9),
                        DsNumeroPlano = reader.IsDBNull(10) ? null : reader.GetString(10),
                        NuConfianzaCategoriaPlano = reader.IsDBNull(11) ? null : reader.GetDecimal(11),
                        NuConfianzaTipoPlano = reader.IsDBNull(12) ? null : reader.GetDecimal(12),
                        NuConfianzaExpediente = reader.IsDBNull(13) ? null : reader.GetDecimal(13),
                        NuConfianzaSeccion = reader.IsDBNull(14) ? null : reader.GetDecimal(14),
                        NuConfianzaManzana = reader.IsDBNull(15) ? null : reader.GetDecimal(15),
                        NuConfianzaParcela = reader.IsDBNull(16) ? null : reader.GetDecimal(16),
                        NuConfianzaDireccion = reader.IsDBNull(17) ? null : reader.GetDecimal(17),
                        NuConfianzaNumeroPlano = reader.IsDBNull(18) ? null : reader.GetDecimal(18),
                        FeAlta = reader.GetDateTime(19),
                        CdUsuarioAlta = reader.GetInt32(20),
                        FeUltimaModificacion = reader.IsDBNull(21) ? null : reader.GetDateTime(21),
                        CdUsuarioModificacion = reader.IsDBNull(22) ? null : reader.GetInt32(22),
                        DsCategoriaPlano = reader.IsDBNull(23) ? null : reader.GetString(23),
                        DsTipoPlano = reader.IsDBNull(24) ? null : reader.GetString(24),
                        NombreArchivo = reader.IsDBNull(25) ? null : reader.GetString(25),
                        NumeroPagina = reader.IsDBNull(26) ? null : reader.GetInt32(26),
                        CdEstadoControl = reader.GetInt32(27),
                        SnModificaDatos = reader.IsDBNull(28) ? null : reader.GetString(28),
                        FeControl = reader.IsDBNull(29) ? null : reader.GetDateTime(29),
                        CdUsuarioControl = reader.IsDBNull(30) ? null : reader.GetInt32(30),
                        DsEstadoControl = reader.IsDBNull(31) ? null : reader.GetString(31)
                    });
                }

                // Si por algún reprocesamiento quedaron múltiples resultados para la misma página,
                // nos quedamos con el más reciente (mayor cdResultado) para evitar duplicar claves
                // al armar diccionarios por cdArchivoPagina en las pantallas que consumen este método.
                return resultados
                    .GroupBy(r => r.CdArchivoPagina)
                    .Select(g => g.OrderByDescending(r => r.CdResultado).First())
                    .ToList();
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "ResultadoIADAL.ObtenerPorLote",
                    DsMensaje = $"Error al obtener resultados del lote {cdLote}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos extraídos (editados manualmente por el usuario) de un resultado IA.
        /// </summary>
        public void ActualizarDatos(ResultadoIA resultado, int cdUsuarioModificacion)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    UPDATE TD_001_RESULTADO_IA
                    SET cdCategoriaPlano = @cdCategoriaPlano,
                        cdTipoPlano = @cdTipoPlano,
                        dsExpediente = @dsExpediente,
                        dsSeccion = @dsSeccion,
                        dsManzana = @dsManzana,
                        dsParcela = @dsParcela,
                        dsDireccion = @dsDireccion,
                        dsNumeroPlano = @dsNumeroPlano,
                        feUltimaModificacion = GETDATE(),
                        cdUsuarioModificacion = @cdUsuarioModificacion
                    WHERE cdResultado = @cdResultado", conn);

                cmd.Parameters.AddWithValue("@cdCategoriaPlano", (object?)resultado.CdCategoriaPlano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cdTipoPlano", (object?)resultado.CdTipoPlano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsExpediente", (object?)resultado.DsExpediente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsSeccion", (object?)resultado.DsSeccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsManzana", (object?)resultado.DsManzana ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsParcela", (object?)resultado.DsParcela ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsDireccion", (object?)resultado.DsDireccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dsNumeroPlano", (object?)resultado.DsNumeroPlano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cdUsuarioModificacion", cdUsuarioModificacion);
                cmd.Parameters.AddWithValue("@cdResultado", resultado.CdResultado);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "ResultadoIADAL.ActualizarDatos",
                    DsMensaje = $"Error al actualizar datos del resultado {resultado.CdResultado}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Actualiza el estado de control de un resultado (Guardar y Marcar Controlada,
        /// Marcar Página Ilegible, Marcar Datos Ilegible), registrando fecha y usuario de control.
        /// </summary>
        public void ActualizarEstadoControl(int cdResultado, int cdEstadoControl, string? snModificaDatos, int cdUsuarioControl)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    UPDATE TD_001_RESULTADO_IA
                    SET cdEstadoControl = @cdEstadoControl,
                        snModificaDatos = @snModificaDatos,
                        feControl = GETDATE(),
                        cdUsuarioControl = @cdUsuarioControl
                    WHERE cdResultado = @cdResultado", conn);

                cmd.Parameters.AddWithValue("@cdEstadoControl", cdEstadoControl);
                cmd.Parameters.AddWithValue("@snModificaDatos", (object?)snModificaDatos ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cdUsuarioControl", cdUsuarioControl);
                cmd.Parameters.AddWithValue("@cdResultado", cdResultado);

                conn.Open();
                cmd.ExecuteNonQuery();

                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "ResultadoIADAL.ActualizarEstadoControl",
                    DsMensaje = $"Resultado IA {cdResultado} actualizado a cdEstadoControl={cdEstadoControl} por usuario {cdUsuarioControl}"
                });
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "ResultadoIADAL.ActualizarEstadoControl",
                    DsMensaje = $"Error al actualizar estado de control del resultado {cdResultado}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
    }
}
