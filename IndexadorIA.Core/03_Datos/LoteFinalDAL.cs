using IndexadorIA.Entidades;
using Microsoft.Data.SqlClient;

namespace IndexadorIA.Datos
{
    /// <summary>Acceso a datos de Lotes Finales (Finalizacion de Planos).</summary>
    public class LoteFinalDAL
    {
        public const int EstadoLoteAsignado = 1;
        public const int EstadoLoteFinalizado = 2;
        public const int EstadoArchivoPendiente = 1;
        public const int EstadoArchivoFinalizado = 2;
        public const int EstadoArchivoDevueltoSmp = 3;

        private readonly string _cadenaConexion;

        public LoteFinalDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Archivos originales cuyas paginas (con resultado IA) estan todas aptas:
        /// cdEstadoControl=2 con validacion SMP vigente (mismos Seccion/Manzana/Parcela),
        /// o cdEstadoControl IN (3,4,7). Excluye los que estan en un lote final pendiente o finalizado.
        /// </summary>
        public List<ArchivoOriginalElegibleDto> ObtenerElegibles()
        {
            const string sql = @"
SELECT a.cdArchivo, a.dsNombreArchivo, a.dsRutaCompleta,
       (SELECT COUNT(*) FROM TD_ARCHIVOS_PAGINAS pp WHERE pp.cdArchivoOriginal = a.cdArchivo) AS nuPaginas,
       (SELECT COUNT(*) FROM TD_ARCHIVOS_PAGINAS pp JOIN TD_001_RESULTADO_IA rr ON rr.cdArchivoPagina = pp.cdArchivoPagina
         WHERE pp.cdArchivoOriginal = a.cdArchivo AND rr.cdEstadoControl IN (2,7)) AS nuCorrectos,
       (SELECT COUNT(*) FROM TD_ARCHIVOS_PAGINAS pp JOIN TD_001_RESULTADO_IA rr ON rr.cdArchivoPagina = pp.cdArchivoPagina
         WHERE pp.cdArchivoOriginal = a.cdArchivo AND rr.cdEstadoControl IN (3,4)) AS nuIlegibles
FROM TD_ARCHIVOS_ORIGINAL a
WHERE EXISTS (SELECT 1 FROM TD_ARCHIVOS_PAGINAS p JOIN TD_001_RESULTADO_IA r ON r.cdArchivoPagina = p.cdArchivoPagina
              WHERE p.cdArchivoOriginal = a.cdArchivo)
  AND NOT EXISTS (
        SELECT 1 FROM TD_ARCHIVOS_PAGINAS p2
        JOIN TD_001_RESULTADO_IA r2 ON r2.cdArchivoPagina = p2.cdArchivoPagina
        WHERE p2.cdArchivoOriginal = a.cdArchivo
          AND NOT ( (r2.cdEstadoControl = 2 AND EXISTS (
                        SELECT 1 FROM TD_SMP_VALIDACION v
                        WHERE v.cdResultado = r2.cdResultado
                          AND ISNULL(v.dsSeccion,'') = ISNULL(r2.dsSeccion,'')
                          AND ISNULL(v.dsManzana,'') = ISNULL(r2.dsManzana,'')
                          AND ISNULL(v.dsParcela,'') = ISNULL(r2.dsParcela,'')))
                    OR r2.cdEstadoControl IN (3,4,7) ))
  AND NOT EXISTS (SELECT 1 FROM TD_LOTE_FINAL_ARCHIVOS lf WHERE lf.cdArchivoOriginal = a.cdArchivo AND lf.cdEstado IN (1,2))
ORDER BY a.dsRutaCompleta, a.cdArchivo";

            var lista = new List<ArchivoOriginalElegibleDto>();
            using var conexion = new SqlConnection(_cadenaConexion);
            conexion.Open();
            using var cmd = new SqlCommand(sql, conexion) { CommandTimeout = 0 };
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new ArchivoOriginalElegibleDto
                {
                    CdArchivo = rd.GetInt32(0),
                    DsNombreArchivo = rd.IsDBNull(1) ? string.Empty : rd.GetString(1),
                    DsRutaCompleta = rd.IsDBNull(2) ? string.Empty : rd.GetString(2),
                    NuPaginas = rd.GetInt32(3),
                    NuCorrectos = rd.GetInt32(4),
                    NuIlegibles = rd.GetInt32(5)
                });
            }
            return lista;
        }

        /// <summary>Crea un lote final con los archivos indicados y lo asigna al usuario. Devuelve cdLoteFinal.</summary>
        public int CrearYAsignar(IReadOnlyCollection<int> cdArchivos, int cdUsuarioAsignado, int cdUsuarioAlta)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            conexion.Open();
            using var tx = conexion.BeginTransaction();

            int cdLote;
            using (var cmd = new SqlCommand(@"
INSERT INTO TD_LOTE_FINAL (dsNombreLote, cdUsuarioAsignado, cdUsuarioAlta, nuCantidadArchivos)
VALUES ('LF_', @asignado, @alta, 0);
DECLARE @id INT = CAST(SCOPE_IDENTITY() AS INT);
UPDATE TD_LOTE_FINAL SET dsNombreLote = 'LF_' + CAST(@id AS VARCHAR(20)) WHERE cdLoteFinal = @id;
SELECT @id;", conexion, tx))
            {
                cmd.Parameters.AddWithValue("@asignado", cdUsuarioAsignado);
                cmd.Parameters.AddWithValue("@alta", cdUsuarioAlta);
                cdLote = Convert.ToInt32(cmd.ExecuteScalar());
            }

            int insertados = 0;
            using (var ins = new SqlCommand(@"
INSERT INTO TD_LOTE_FINAL_ARCHIVOS (cdLoteFinal, cdArchivoOriginal, cdEstado)
SELECT @lote, @archivo, 1
WHERE NOT EXISTS (SELECT 1 FROM TD_LOTE_FINAL_ARCHIVOS WHERE cdArchivoOriginal = @archivo AND cdEstado IN (1,2))", conexion, tx))
            {
                var pLote = ins.Parameters.Add("@lote", System.Data.SqlDbType.Int);
                var pArch = ins.Parameters.Add("@archivo", System.Data.SqlDbType.Int);
                pLote.Value = cdLote;
                foreach (int cd in cdArchivos)
                {
                    pArch.Value = cd;
                    insertados += ins.ExecuteNonQuery();
                }
            }

            if (insertados == 0)
            {
                tx.Rollback();
                throw new InvalidOperationException("Los archivos seleccionados ya fueron asignados a otro lote.");
            }

            using (var upd = new SqlCommand("UPDATE TD_LOTE_FINAL SET nuCantidadArchivos=@n WHERE cdLoteFinal=@l", conexion, tx))
            {
                upd.Parameters.AddWithValue("@n", insertados);
                upd.Parameters.AddWithValue("@l", cdLote);
                upd.ExecuteNonQuery();
            }

            tx.Commit();
            return cdLote;
        }
    }
}
