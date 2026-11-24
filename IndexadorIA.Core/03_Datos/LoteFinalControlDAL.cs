using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>Acceso a datos del Control Final (Data Entry) sobre archivos originales de un lote final.</summary>
    public class LoteFinalControlDAL
    {
        private readonly string _cadenaConexion = Configuracion.CadenaConexion;

        public List<MonitorLoteFinalUsuarioDto> ObtenerResumenPorUsuario()
        {
            const string sql = @"
SELECT u.cdUsuario, u.dsUsuario,
       COUNT(DISTINCT l.cdLoteFinal),
       COUNT(DISTINCT CASE WHEN l.cdEstadoLote = 1 THEN l.cdLoteFinal END),
       COUNT(a.cdLoteFinalArchivo),
       SUM(CASE WHEN a.cdEstado = 1 THEN 1 ELSE 0 END),
       SUM(CASE WHEN a.cdEstado = 2 THEN 1 ELSE 0 END),
       SUM(CASE WHEN a.cdEstado = 3 THEN 1 ELSE 0 END)
FROM TD_LOTE_FINAL l
JOIN TD_USUARIOS u ON u.cdUsuario = l.cdUsuarioAsignado
LEFT JOIN TD_LOTE_FINAL_ARCHIVOS a ON a.cdLoteFinal = l.cdLoteFinal
GROUP BY u.cdUsuario, u.dsUsuario
ORDER BY u.dsUsuario";
            var lista = new List<MonitorLoteFinalUsuarioDto>();
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new MonitorLoteFinalUsuarioDto
                {
                    CdUsuario = rd.GetInt32(0),
                    DsUsuario = rd.GetString(1),
                    NuLotes = rd.GetInt32(2),
                    NuLotesAsignados = rd.GetInt32(3),
                    NuArchivos = rd.GetInt32(4),
                    NuPendientes = rd.IsDBNull(5) ? 0 : rd.GetInt32(5),
                    NuFinalizados = rd.IsDBNull(6) ? 0 : rd.GetInt32(6),
                    NuDevueltosSmp = rd.IsDBNull(7) ? 0 : rd.GetInt32(7)
                });
            }
            return lista;
        }

        /// <summary>Todos los lotes (cualquier estado) asignados a un usuario.</summary>
        public List<LoteFinalDto> ObtenerLotesPorUsuario(int cdUsuario)
        {
            const string sql = @"
SELECT l.cdLoteFinal, l.dsNombreLote, l.cdUsuarioAsignado, u.dsUsuario, l.cdEstadoLote, l.nuCantidadArchivos,
       (SELECT COUNT(*) FROM TD_LOTE_FINAL_ARCHIVOS a WHERE a.cdLoteFinal = l.cdLoteFinal AND a.cdEstado = 1),
       (SELECT COUNT(*) FROM TD_LOTE_FINAL_ARCHIVOS a WHERE a.cdLoteFinal = l.cdLoteFinal AND a.cdEstado = 2),
       l.feAlta
FROM TD_LOTE_FINAL l
JOIN TD_USUARIOS u ON u.cdUsuario = l.cdUsuarioAsignado
WHERE l.cdUsuarioAsignado = @u
ORDER BY l.cdLoteFinal DESC";
            var lista = new List<LoteFinalDto>();
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@u", cdUsuario);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new LoteFinalDto
                {
                    CdLoteFinal = rd.GetInt32(0),
                    DsNombreLote = rd.GetString(1),
                    CdUsuarioAsignado = rd.GetInt32(2),
                    DsUsuarioAsignado = rd.GetString(3),
                    CdEstadoLote = rd.GetInt32(4),
                    NuArchivos = rd.GetInt32(5),
                    NuPendientes = rd.GetInt32(6),
                    NuFinalizados = rd.GetInt32(7),
                    FeAlta = rd.GetDateTime(8)
                });
            }
            return lista;
        }

        /// <summary>Reasigna lotes asignados (no finalizados) a otro usuario. Devuelve la cantidad reasignada.</summary>
        public int Reasignar(IReadOnlyCollection<int> cdLotes, int cdUsuarioNuevo)
        {
            int total = 0;
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var tx = cn.BeginTransaction();
            foreach (int cd in cdLotes)
            {
                using var cmd = new SqlCommand(@"
UPDATE TD_LOTE_FINAL SET cdUsuarioAsignado = @n, feUltimoCambio = GETDATE()
WHERE cdLoteFinal = @l AND cdEstadoLote = 1 AND cdUsuarioAsignado <> @n", cn, tx);
                cmd.Parameters.AddWithValue("@n", cdUsuarioNuevo);
                cmd.Parameters.AddWithValue("@l", cd);
                total += cmd.ExecuteNonQuery();
            }
            tx.Commit();
            return total;
        }

        public List<LoteFinalDto> ObtenerLotesDeUsuario(int cdUsuario)
        {
            const string sql = @"
SELECT l.cdLoteFinal, l.dsNombreLote, l.cdUsuarioAsignado, u.dsUsuario, l.cdEstadoLote, l.nuCantidadArchivos,
       (SELECT COUNT(*) FROM TD_LOTE_FINAL_ARCHIVOS a WHERE a.cdLoteFinal = l.cdLoteFinal AND a.cdEstado = 1),
       (SELECT COUNT(*) FROM TD_LOTE_FINAL_ARCHIVOS a WHERE a.cdLoteFinal = l.cdLoteFinal AND a.cdEstado = 2),
       l.feAlta
FROM TD_LOTE_FINAL l
JOIN TD_USUARIOS u ON u.cdUsuario = l.cdUsuarioAsignado
WHERE l.cdUsuarioAsignado = @u AND l.cdEstadoLote = 1
ORDER BY l.cdLoteFinal";
            var lista = new List<LoteFinalDto>();
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@u", cdUsuario);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new LoteFinalDto
                {
                    CdLoteFinal = rd.GetInt32(0),
                    DsNombreLote = rd.GetString(1),
                    CdUsuarioAsignado = rd.GetInt32(2),
                    DsUsuarioAsignado = rd.GetString(3),
                    CdEstadoLote = rd.GetInt32(4),
                    NuArchivos = rd.GetInt32(5),
                    NuPendientes = rd.GetInt32(6),
                    NuFinalizados = rd.GetInt32(7),
                    FeAlta = rd.GetDateTime(8)
                });
            }
            return lista;
        }

        public List<ArchivoLoteFinalDto> ObtenerArchivosDeLote(int cdLoteFinal)
        {
            const string sql = @"
SELECT o.cdArchivo, o.dsNombreArchivo, o.dsRutaCompleta, la.cdEstado,
       (SELECT COUNT(*) FROM TD_ARCHIVOS_PAGINAS p WHERE p.cdArchivoOriginal = o.cdArchivo),
       (SELECT COUNT(*) FROM TD_ARCHIVOS_PAGINAS p JOIN TD_LOTE_FINAL_PAGINAS f ON f.cdArchivoPagina = p.cdArchivoPagina
         WHERE p.cdArchivoOriginal = o.cdArchivo)
FROM TD_LOTE_FINAL_ARCHIVOS la
JOIN TD_ARCHIVOS_ORIGINAL o ON o.cdArchivo = la.cdArchivoOriginal
WHERE la.cdLoteFinal = @l
ORDER BY la.cdEstado, o.dsRutaCompleta";
            var lista = new List<ArchivoLoteFinalDto>();
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@l", cdLoteFinal);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new ArchivoLoteFinalDto
                {
                    CdArchivo = rd.GetInt32(0),
                    DsNombreArchivo = rd.IsDBNull(1) ? string.Empty : rd.GetString(1),
                    DsRutaCompleta = rd.IsDBNull(2) ? string.Empty : rd.GetString(2),
                    CdEstado = rd.GetInt32(3),
                    NuPaginas = rd.GetInt32(4),
                    NuFinalizadas = rd.GetInt32(5)
                });
            }
            return lista;
        }

        /// <summary>Paginas del original que todavia no fueron incluidas en un PDF final.</summary>
        public List<PaginaFinalDto> ObtenerPaginas(int cdArchivoOriginal)
        {
            const string sql = @"
SELECT r.cdResultado, p.cdArchivoPagina, p.nuPagina, p.dsRutaCompleta, r.cdEstadoControl, e.dsEstado,
       r.cdCategoriaPlano, r.cdTipoPlano, cp.dsCategoriaPlano, tp.dsTipoPlano, tp.dsAcronimo,
       r.dsExpediente, r.dsSeccion, r.dsManzana, r.dsParcela, r.dsDireccion, r.dsNumeroPlano, r.dsObservaciones
FROM TD_ARCHIVOS_PAGINAS p
JOIN TD_001_RESULTADO_IA r ON r.cdArchivoPagina = p.cdArchivoPagina
LEFT JOIN TD_CATEGORIA_PLANO cp ON cp.cdCategoriaPlano = r.cdCategoriaPlano
LEFT JOIN TD_TIPOS_PLANO tp ON tp.cdTipoPlano = r.cdTipoPlano
LEFT JOIN TD_ESTADOS e ON e.dsProceso = 'CONTROL' AND e.cdEstado = r.cdEstadoControl
WHERE p.cdArchivoOriginal = @a
  AND NOT EXISTS (SELECT 1 FROM TD_LOTE_FINAL_PAGINAS f WHERE f.cdArchivoPagina = p.cdArchivoPagina)
ORDER BY p.nuPagina";
            var lista = new List<PaginaFinalDto>();
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@a", cdArchivoOriginal);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                string S(int i) => rd.IsDBNull(i) ? string.Empty : rd.GetString(i);
                lista.Add(new PaginaFinalDto
                {
                    CdResultado = rd.GetInt32(0),
                    CdArchivoPagina = rd.GetInt32(1),
                    NuPagina = rd.GetInt32(2),
                    DsRutaPagina = S(3),
                    CdEstadoControl = rd.IsDBNull(4) ? 0 : rd.GetInt32(4),
                    DsEstadoControl = S(5),
                    CdCategoriaPlano = rd.IsDBNull(6) ? null : rd.GetInt32(6),
                    CdTipoPlano = rd.IsDBNull(7) ? null : rd.GetInt32(7),
                    DsCategoria = S(8),
                    DsTipo = S(9),
                    DsAcronimo = S(10),
                    DsExpediente = S(11),
                    DsSeccion = S(12),
                    DsManzana = S(13),
                    DsParcela = S(14),
                    DsDireccion = S(15),
                    DsNumeroPlano = S(16),
                    DsObservaciones = S(17)
                });
            }
            return lista;
        }

        public void GuardarPagina(PaginaFinalDto p, int cdUsuario)
        {
            const string sql = @"
UPDATE TD_001_RESULTADO_IA
SET cdCategoriaPlano = @cat, cdTipoPlano = @tipo, cdEstadoControl = @est, dsExpediente = @exp, dsSeccion = @sec, dsManzana = @man,
    dsParcela = @par, dsDireccion = @dir, dsNumeroPlano = @nro, dsObservaciones = @obs,
    feUltimaModificacion = GETDATE(), cdUsuarioModificacion = @usr
WHERE cdResultado = @res";
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@cat", (object?)p.CdCategoriaPlano ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@tipo", (object?)p.CdTipoPlano ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@est", p.CdEstadoControl > 0 ? p.CdEstadoControl : DBNull.Value);
            cmd.Parameters.AddWithValue("@exp", Nulo(p.DsExpediente));
            cmd.Parameters.AddWithValue("@sec", Nulo(p.DsSeccion));
            cmd.Parameters.AddWithValue("@man", Nulo(p.DsManzana));
            cmd.Parameters.AddWithValue("@par", Nulo(p.DsParcela));
            cmd.Parameters.AddWithValue("@dir", Nulo(p.DsDireccion));
            cmd.Parameters.AddWithValue("@nro", Nulo(p.DsNumeroPlano));
            cmd.Parameters.AddWithValue("@obs", Nulo(p.DsObservaciones));
            cmd.Parameters.AddWithValue("@usr", cdUsuario);
            cmd.Parameters.AddWithValue("@res", p.CdResultado);
            cmd.ExecuteNonQuery();
        }

        private static object Nulo(string? s) => string.IsNullOrWhiteSpace(s) ? DBNull.Value : s.Trim();

        public List<EstadoControlItem> ObtenerEstadosControl()
        {
            var lista = new List<EstadoControlItem>();
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var cmd = new SqlCommand("SELECT cdEstado, dsEstado FROM TD_ESTADOS WHERE dsProceso = 'CONTROL' ORDER BY cdEstado", cn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new EstadoControlItem { Codigo = rd.GetInt32(0), Descripcion = rd.GetString(1) });
            }
            return lista;
        }

        /// <summary>Resultado de la validacion SMP (API) del registro, con sus parcelas vigentes.</summary>
        public SmpValidacion? ObtenerValidacionSmp(int cdResultado)
        {
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            SmpValidacion? v = null;
            int cdValidacion = 0;
            using (var cmd = new SqlCommand(@"
SELECT cdValidacion, dsSeccion, dsManzana, dsParcela, dsSmpConsultado, dsEstado, dsMensaje, dsMotivoOmision, feConsulta
FROM TD_SMP_VALIDACION WHERE cdResultado = @r", cn))
            {
                cmd.Parameters.AddWithValue("@r", cdResultado);
                using var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    cdValidacion = rd.GetInt32(0);
                    v = new SmpValidacion
                    {
                        CdResultado = cdResultado,
                        DsSeccion = rd.IsDBNull(1) ? null : rd.GetString(1),
                        DsManzana = rd.IsDBNull(2) ? null : rd.GetString(2),
                        DsParcela = rd.IsDBNull(3) ? null : rd.GetString(3),
                        DsSmp = rd.IsDBNull(4) ? null : rd.GetString(4),
                        DsEstado = rd.IsDBNull(5) ? string.Empty : rd.GetString(5),
                        DsMensaje = rd.IsDBNull(6) ? null : rd.GetString(6),
                        DsMotivoOmision = rd.IsDBNull(7) ? null : rd.GetString(7),
                        FeConsulta = rd.IsDBNull(8) ? null : rd.GetDateTime(8)
                    };
                }
            }
            if (v == null) return null;

            using (var cmd = new SqlCommand(@"
SELECT dsSmp, dsSurgeDe, feRegistro, dsCaso, dsObservacion, dsDirecciones
FROM TD_SMP_VALIDACION_PARCELA WHERE cdValidacion = @v", cn))
            {
                cmd.Parameters.AddWithValue("@v", cdValidacion);
                using var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    v.Parcelas.Add(new SmpParcelaVigente
                    {
                        DsSmp = rd.IsDBNull(0) ? string.Empty : rd.GetString(0),
                        DsSurgeDe = rd.IsDBNull(1) ? null : rd.GetString(1),
                        FeRegistro = rd.IsDBNull(2) ? null : rd.GetDateTime(2),
                        DsCaso = rd.IsDBNull(3) ? null : rd.GetString(3),
                        DsObservacion = rd.IsDBNull(4) ? null : rd.GetString(4),
                        DsDirecciones = rd.IsDBNull(5) ? null : rd.GetString(5)
                    });
                }
            }
            return v;
        }

        /// <summary>
        /// Registra las paginas incluidas en un PDF final. Si ya no quedan paginas pendientes del original
        /// lo marca finalizado, y si no quedan originales pendientes en el lote, finaliza el lote.
        /// </summary>
        public void RegistrarFinalizacion(int cdLoteFinal, int cdArchivoOriginal, IEnumerable<int> cdPaginas,
            string nombreFinal, string rutaFinal, bool ilegible, int cdUsuario)
        {
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var tx = cn.BeginTransaction();

            foreach (int pag in cdPaginas)
            {
                using var ins = new SqlCommand(@"
INSERT INTO TD_LOTE_FINAL_PAGINAS (cdLoteFinal, cdArchivoPagina, dsNombreArchivoFinal, dsRutaArchivoFinal, snIlegible, cdUsuario)
VALUES (@l, @p, @n, @r, @i, @u)", cn, tx);
                ins.Parameters.AddWithValue("@l", cdLoteFinal);
                ins.Parameters.AddWithValue("@p", pag);
                ins.Parameters.AddWithValue("@n", nombreFinal);
                ins.Parameters.AddWithValue("@r", rutaFinal);
                ins.Parameters.AddWithValue("@i", ilegible ? "SI" : "NO");
                ins.Parameters.AddWithValue("@u", cdUsuario);
                ins.ExecuteNonQuery();
            }

            using (var upd = new SqlCommand(@"
UPDATE TD_LOTE_FINAL_ARCHIVOS SET cdEstado = 2, feFinalizacion = GETDATE(), feUltimoCambio = GETDATE()
WHERE cdLoteFinal = @l AND cdArchivoOriginal = @a AND cdEstado = 1
  AND NOT EXISTS (SELECT 1 FROM TD_ARCHIVOS_PAGINAS p
                  WHERE p.cdArchivoOriginal = @a
                    AND NOT EXISTS (SELECT 1 FROM TD_LOTE_FINAL_PAGINAS f WHERE f.cdArchivoPagina = p.cdArchivoPagina))", cn, tx))
            {
                upd.Parameters.AddWithValue("@l", cdLoteFinal);
                upd.Parameters.AddWithValue("@a", cdArchivoOriginal);
                upd.ExecuteNonQuery();
            }

            FinalizarLoteSiCorresponde(cn, tx, cdLoteFinal);
            tx.Commit();
        }

        /// <summary>
        /// Devuelve el original para re-validar contra la API SMP: elimina su validacion vigente (para que el
        /// validador lo tome como pendiente) y lo marca como devuelto (queda disponible en Crear Lotes Final).
        /// </summary>
        public void DevolverParaActualizarSmp(int cdLoteFinal, int cdArchivoOriginal)
        {
            using var cn = new SqlConnection(_cadenaConexion);
            cn.Open();
            using var tx = cn.BeginTransaction();

            const string filtro = @"
(SELECT r.cdResultado FROM TD_001_RESULTADO_IA r
 JOIN TD_ARCHIVOS_PAGINAS p ON p.cdArchivoPagina = r.cdArchivoPagina
 WHERE p.cdArchivoOriginal = @a)";
            foreach (string sql in new[]
            {
                $"DELETE FROM TD_SMP_VALIDACION_PARCELA WHERE cdValidacion IN (SELECT cdValidacion FROM TD_SMP_VALIDACION WHERE cdResultado IN {filtro})",
                $"DELETE FROM TD_SMP_VALIDACION WHERE cdResultado IN {filtro}"
            })
            {
                using var del = new SqlCommand(sql, cn, tx);
                del.Parameters.AddWithValue("@a", cdArchivoOriginal);
                del.ExecuteNonQuery();
            }

            using (var upd = new SqlCommand(@"
UPDATE TD_LOTE_FINAL_ARCHIVOS SET cdEstado = 3, feUltimoCambio = GETDATE()
WHERE cdLoteFinal = @l AND cdArchivoOriginal = @a AND cdEstado = 1", cn, tx))
            {
                upd.Parameters.AddWithValue("@l", cdLoteFinal);
                upd.Parameters.AddWithValue("@a", cdArchivoOriginal);
                upd.ExecuteNonQuery();
            }

            FinalizarLoteSiCorresponde(cn, tx, cdLoteFinal);
            tx.Commit();
        }

        private static void FinalizarLoteSiCorresponde(SqlConnection cn, SqlTransaction tx, int cdLoteFinal)
        {
            using var cmd = new SqlCommand(@"
UPDATE TD_LOTE_FINAL SET cdEstadoLote = 2, feFinalizacion = GETDATE(), feUltimoCambio = GETDATE()
WHERE cdLoteFinal = @l AND cdEstadoLote = 1
  AND NOT EXISTS (SELECT 1 FROM TD_LOTE_FINAL_ARCHIVOS WHERE cdLoteFinal = @l AND cdEstado = 1)", cn, tx);
            cmd.Parameters.AddWithValue("@l", cdLoteFinal);
            cmd.ExecuteNonQuery();
        }
    }
}
