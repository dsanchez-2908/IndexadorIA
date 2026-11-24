using IndexadorIA.Entidades;
using IndexadorIA.Utilidades;
using Microsoft.Data.Sqlite;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Cola de trabajo local (SQLite) del Validador SMP. Permite consultar la API
    /// (VPN FortiClient) sin necesitar conexion a la base de datos (VPN OpenVPN).
    /// </summary>
    public class SmpLocalStore
    {
        private readonly string _cadena;

        public static string RutaDefault => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "IndexadorIA", "smp_validacion.db");

        public SmpLocalStore(string? ruta = null)
        {
            ruta ??= RutaDefault;
            Directory.CreateDirectory(Path.GetDirectoryName(ruta)!);
            _cadena = $"Data Source={ruta}";
            Inicializar();
        }

        private SqliteConnection Abrir()
        {
            var c = new SqliteConnection(_cadena);
            c.Open();
            return c;
        }

        private void Inicializar()
        {
            using var c = Abrir();
            using var cmd = c.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS SMP_COLA (
    cdResultado INTEGER PRIMARY KEY,
    cdLote INTEGER NULL,
    cdArchivoPagina INTEGER NULL,
    dsSeccion TEXT NULL,
    dsManzana TEXT NULL,
    dsParcela TEXT NULL,
    dsSmp TEXT NULL,
    dsEstado TEXT NOT NULL,
    dsMensaje TEXT NULL,
    dsMotivoOmision TEXT NULL,
    nuHttpStatus INTEGER NULL,
    dsJson TEXT NULL,
    feConsulta TEXT NULL,
    snSincronizado INTEGER NOT NULL DEFAULT 0
);
CREATE INDEX IF NOT EXISTS IX_SMP_COLA_estado ON SMP_COLA (dsEstado, snSincronizado);";
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Incorpora registros descargados. Si ya existe uno con los mismos datos S/M/P se conserva
        /// (no pisa resultados locales); si cambiaron, se reinicia como pendiente/omitido.
        /// </summary>
        public void Incorporar(IEnumerable<SmpPendiente> registros)
        {
            using var c = Abrir();
            using var tx = c.BeginTransaction();
            using var cmd = c.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = @"
INSERT INTO SMP_COLA (cdResultado, cdLote, cdArchivoPagina, dsSeccion, dsManzana, dsParcela, dsSmp, dsEstado, dsMotivoOmision, snSincronizado)
VALUES ($r, $l, $a, $s, $m, $p, $smp, $e, $mo, $sync)
ON CONFLICT(cdResultado) DO UPDATE SET
    cdLote = excluded.cdLote, cdArchivoPagina = excluded.cdArchivoPagina,
    dsSeccion = excluded.dsSeccion, dsManzana = excluded.dsManzana, dsParcela = excluded.dsParcela,
    dsSmp = excluded.dsSmp, dsEstado = excluded.dsEstado, dsMotivoOmision = excluded.dsMotivoOmision,
    dsMensaje = NULL, nuHttpStatus = NULL, dsJson = NULL, feConsulta = NULL, snSincronizado = excluded.snSincronizado
WHERE IFNULL(SMP_COLA.dsSeccion,'') <> IFNULL(excluded.dsSeccion,'')
   OR IFNULL(SMP_COLA.dsManzana,'') <> IFNULL(excluded.dsManzana,'')
   OR IFNULL(SMP_COLA.dsParcela,'') <> IFNULL(excluded.dsParcela,'')";
            var pr = cmd.Parameters.Add("$r", SqliteType.Integer);
            var pl = cmd.Parameters.Add("$l", SqliteType.Integer);
            var pa = cmd.Parameters.Add("$a", SqliteType.Integer);
            var ps = cmd.Parameters.Add("$s", SqliteType.Text);
            var pm = cmd.Parameters.Add("$m", SqliteType.Text);
            var pp = cmd.Parameters.Add("$p", SqliteType.Text);
            var psmp = cmd.Parameters.Add("$smp", SqliteType.Text);
            var pe = cmd.Parameters.Add("$e", SqliteType.Text);
            var pmo = cmd.Parameters.Add("$mo", SqliteType.Text);
            var psync = cmd.Parameters.Add("$sync", SqliteType.Integer);

            foreach (var x in registros)
            {
                SmpHelper.Evaluar(x);
                pr.Value = x.CdResultado;
                pl.Value = (object?)x.CdLote ?? DBNull.Value;
                pa.Value = (object?)x.CdArchivoPagina ?? DBNull.Value;
                ps.Value = (object?)x.DsSeccion ?? DBNull.Value;
                pm.Value = (object?)x.DsManzana ?? DBNull.Value;
                pp.Value = (object?)x.DsParcela ?? DBNull.Value;
                psmp.Value = (object?)x.DsSmp ?? DBNull.Value;
                pe.Value = x.DsSmp == null ? SmpEstado.Omitido : SmpEstado.Pendiente;
                pmo.Value = (object?)x.DsMotivoOmision ?? DBNull.Value;
                psync.Value = 0;
                cmd.ExecuteNonQuery();
            }
            tx.Commit();
        }

        public List<SmpPendiente> ObtenerPendientes(int max)
        {
            var lista = new List<SmpPendiente>();
            using var c = Abrir();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $"SELECT cdResultado, cdLote, cdArchivoPagina, dsSeccion, dsManzana, dsParcela, dsSmp FROM SMP_COLA WHERE dsEstado = '{SmpEstado.Pendiente}' ORDER BY cdResultado LIMIT $max";
            cmd.Parameters.AddWithValue("$max", max);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new SmpPendiente
                {
                    CdResultado = rd.GetInt32(0),
                    CdLote = rd.IsDBNull(1) ? null : rd.GetInt32(1),
                    CdArchivoPagina = rd.IsDBNull(2) ? null : rd.GetInt32(2),
                    DsSeccion = rd.IsDBNull(3) ? null : rd.GetString(3),
                    DsManzana = rd.IsDBNull(4) ? null : rd.GetString(4),
                    DsParcela = rd.IsDBNull(5) ? null : rd.GetString(5),
                    DsSmp = rd.IsDBNull(6) ? null : rd.GetString(6)
                });
            }
            return lista;
        }

        public void GuardarResultado(SmpValidacion v)
        {
            using var c = Abrir();
            using var cmd = c.CreateCommand();
            cmd.CommandText = @"UPDATE SMP_COLA SET dsEstado=$e, dsMensaje=$m, nuHttpStatus=$h, dsJson=$j, feConsulta=$f, snSincronizado=0
                                WHERE cdResultado=$r";
            cmd.Parameters.AddWithValue("$e", v.DsEstado);
            cmd.Parameters.AddWithValue("$m", (object?)v.DsMensaje ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$h", (object?)v.NuHttpStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$j", (object?)v.DsJson ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$f", (v.FeConsulta ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$r", v.CdResultado);
            cmd.ExecuteNonQuery();
        }

        /// <summary>Registros con resultado (consultados u omitidos) aun no subidos a la BD.</summary>
        public List<SmpValidacion> ObtenerSinSincronizar(int max)
        {
            var lista = new List<SmpValidacion>();
            using var c = Abrir();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $@"SELECT cdResultado, cdLote, cdArchivoPagina, dsSeccion, dsManzana, dsParcela, dsSmp,
                                        dsEstado, dsMensaje, dsMotivoOmision, nuHttpStatus, dsJson, feConsulta
                                 FROM SMP_COLA WHERE snSincronizado = 0 AND dsEstado <> '{SmpEstado.Pendiente}'
                                 ORDER BY cdResultado LIMIT $max";
            cmd.Parameters.AddWithValue("$max", max);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                var v = new SmpValidacion
                {
                    CdResultado = rd.GetInt32(0),
                    CdLote = rd.IsDBNull(1) ? null : rd.GetInt32(1),
                    CdArchivoPagina = rd.IsDBNull(2) ? null : rd.GetInt32(2),
                    DsSeccion = rd.IsDBNull(3) ? null : rd.GetString(3),
                    DsManzana = rd.IsDBNull(4) ? null : rd.GetString(4),
                    DsParcela = rd.IsDBNull(5) ? null : rd.GetString(5),
                    DsSmp = rd.IsDBNull(6) ? null : rd.GetString(6),
                    DsEstado = rd.GetString(7),
                    DsMensaje = rd.IsDBNull(8) ? null : rd.GetString(8),
                    DsMotivoOmision = rd.IsDBNull(9) ? null : rd.GetString(9),
                    NuHttpStatus = rd.IsDBNull(10) ? null : rd.GetInt32(10),
                    DsJson = rd.IsDBNull(11) ? null : rd.GetString(11),
                    FeConsulta = rd.IsDBNull(12) ? null : DateTime.Parse(rd.GetString(12))
                };
                if (!string.IsNullOrEmpty(v.DsJson))
                {
                    v.Parcelas = Negocio.SmpApiClient.Parsear(v.DsJson).Parcelas;
                }
                lista.Add(v);
            }
            return lista;
        }

        public void MarcarSincronizados(IEnumerable<int> cdResultados)
        {
            using var c = Abrir();
            using var tx = c.BeginTransaction();
            using var cmd = c.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "UPDATE SMP_COLA SET snSincronizado = 1 WHERE cdResultado = $r";
            var p = cmd.Parameters.Add("$r", SqliteType.Integer);
            foreach (var id in cdResultados)
            {
                p.Value = id;
                cmd.ExecuteNonQuery();
            }
            tx.Commit();
        }

        public SmpContadores ObtenerContadores()
        {
            using var c = Abrir();
            using var cmd = c.CreateCommand();
            cmd.CommandText = $@"SELECT COUNT(*),
                SUM(CASE WHEN dsEstado NOT IN ('{SmpEstado.Pendiente}','{SmpEstado.Omitido}') THEN 1 ELSE 0 END),
                SUM(CASE WHEN dsEstado = '{SmpEstado.Pendiente}' THEN 1 ELSE 0 END),
                SUM(CASE WHEN dsEstado = '{SmpEstado.Omitido}' THEN 1 ELSE 0 END),
                SUM(CASE WHEN snSincronizado = 0 AND dsEstado <> '{SmpEstado.Pendiente}' THEN 1 ELSE 0 END)
                FROM SMP_COLA";
            using var rd = cmd.ExecuteReader();
            rd.Read();
            return new SmpContadores
            {
                Total = rd.GetInt32(0),
                Validados = rd.IsDBNull(1) ? 0 : rd.GetInt32(1),
                Pendientes = rd.IsDBNull(2) ? 0 : rd.GetInt32(2),
                Omitidos = rd.IsDBNull(3) ? 0 : rd.GetInt32(3),
                SinSincronizar = rd.IsDBNull(4) ? 0 : rd.GetInt32(4)
            };
        }
    }
}
