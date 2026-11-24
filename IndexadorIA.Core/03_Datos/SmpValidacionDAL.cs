using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>Acceso a SQL Server para el Validador API SMP (requiere VPN de base de datos).</summary>
    public class SmpValidacionDAL
    {
        private readonly string _cadenaConexion;

        public SmpValidacionDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Registros de TD_001_RESULTADO_IA sin validacion registrada, o cuya seccion/manzana/parcela
        /// cambio desde la ultima validacion (reproceso por correccion). Lectura en streaming.
        /// </summary>
        public IEnumerable<SmpPendiente> ObtenerPendientes()
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            conexion.Open();
            using var cmd = new SqlCommand(@"
SELECT r.cdResultado, r.cdLote, r.cdArchivoPagina, r.dsSeccion, r.dsManzana, r.dsParcela
FROM TD_001_RESULTADO_IA r
LEFT JOIN TD_SMP_VALIDACION v ON v.cdResultado = r.cdResultado
WHERE v.cdResultado IS NULL
   OR ISNULL(v.dsSeccion, '') <> ISNULL(r.dsSeccion, '')
   OR ISNULL(v.dsManzana, '') <> ISNULL(r.dsManzana, '')
   OR ISNULL(v.dsParcela, '') <> ISNULL(r.dsParcela, '')", conexion)
            { CommandTimeout = 0 };

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                yield return new SmpPendiente
                {
                    CdResultado = rd.GetInt32(0),
                    CdLote = rd.IsDBNull(1) ? null : rd.GetInt32(1),
                    CdArchivoPagina = rd.IsDBNull(2) ? null : rd.GetInt32(2),
                    DsSeccion = rd.IsDBNull(3) ? null : rd.GetString(3),
                    DsManzana = rd.IsDBNull(4) ? null : rd.GetString(4),
                    DsParcela = rd.IsDBNull(5) ? null : rd.GetString(5)
                };
            }
        }

        /// <summary>Guarda (upsert por cdResultado) un lote de validaciones con sus parcelas en una transaccion.</summary>
        public void Guardar(IEnumerable<SmpValidacion> validaciones)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            conexion.Open();
            using var tx = conexion.BeginTransaction();

            foreach (var v in validaciones)
            {
                int cdValidacion;
                using (var cmd = new SqlCommand(@"
UPDATE TD_SMP_VALIDACION SET
    cdLote=@cdLote, cdArchivoPagina=@cdArchivoPagina, dsSeccion=@s, dsManzana=@m, dsParcela=@p,
    dsSmpConsultado=@smp, dsEstado=@estado, dsMensaje=@mensaje, dsMotivoOmision=@motivo,
    nuHttpStatus=@http, dsJson=@json, feConsulta=@fe, feModificacion=GETDATE()
WHERE cdResultado=@cdResultado;
IF @@ROWCOUNT = 0
    INSERT INTO TD_SMP_VALIDACION (cdResultado, cdLote, cdArchivoPagina, dsSeccion, dsManzana, dsParcela,
        dsSmpConsultado, dsEstado, dsMensaje, dsMotivoOmision, nuHttpStatus, dsJson, feConsulta)
    VALUES (@cdResultado, @cdLote, @cdArchivoPagina, @s, @m, @p, @smp, @estado, @mensaje, @motivo, @http, @json, @fe);
SELECT cdValidacion FROM TD_SMP_VALIDACION WHERE cdResultado=@cdResultado;", conexion, tx))
                {
                    cmd.Parameters.AddWithValue("@cdResultado", v.CdResultado);
                    cmd.Parameters.AddWithValue("@cdLote", (object?)v.CdLote ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@cdArchivoPagina", (object?)v.CdArchivoPagina ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@s", (object?)v.DsSeccion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@m", (object?)v.DsManzana ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p", (object?)v.DsParcela ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@smp", (object?)v.DsSmp ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@estado", v.DsEstado);
                    cmd.Parameters.AddWithValue("@mensaje", (object?)v.DsMensaje ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@motivo", (object?)v.DsMotivoOmision ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@http", (object?)v.NuHttpStatus ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@json", (object?)v.DsJson ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@fe", (object?)v.FeConsulta ?? DBNull.Value);
                    cdValidacion = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var del = new SqlCommand("DELETE FROM TD_SMP_VALIDACION_PARCELA WHERE cdValidacion=@id", conexion, tx))
                {
                    del.Parameters.AddWithValue("@id", cdValidacion);
                    del.ExecuteNonQuery();
                }

                foreach (var p in v.Parcelas)
                {
                    using var ins = new SqlCommand(@"
INSERT INTO TD_SMP_VALIDACION_PARCELA (cdValidacion, dsSmp, dsSurgeDe, feRegistro, dsCaso, dsObservacion, dsDirecciones)
VALUES (@id, @smp, @surge, @fe, @caso, @obs, @dir)", conexion, tx);
                    ins.Parameters.AddWithValue("@id", cdValidacion);
                    ins.Parameters.AddWithValue("@smp", p.DsSmp);
                    ins.Parameters.AddWithValue("@surge", (object?)p.DsSurgeDe ?? DBNull.Value);
                    ins.Parameters.Add("@fe", System.Data.SqlDbType.Date).Value = (object?)p.FeRegistro ?? DBNull.Value;
                    ins.Parameters.AddWithValue("@caso", (object?)p.DsCaso ?? DBNull.Value);
                    ins.Parameters.AddWithValue("@obs", (object?)p.DsObservacion ?? DBNull.Value);
                    ins.Parameters.AddWithValue("@dir", (object?)p.DsDirecciones ?? DBNull.Value);
                    ins.ExecuteNonQuery();
                }
            }

            tx.Commit();
        }
    }
}
