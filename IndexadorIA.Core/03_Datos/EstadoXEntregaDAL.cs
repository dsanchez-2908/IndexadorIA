using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para el reporte "Estado x Entrega": resume, por entrega
    /// (TD_ARCHIVOS_PAGINAS.dsEntrega), la cantidad de lotes y planos totales, pendientes
    /// (cdEstadoLote IN 1..10) y enviados (cdEstadoLote = 11). Tambien permite consultar,
    /// para una entrega dada, la agrupacion de lotes pendientes por estado y el detalle de
    /// lotes filtrado por entrega y estado.
    /// </summary>
    public class EstadoXEntregaDAL
    {
        private readonly string _cadenaConexion;

        /// <summary>
        /// Estados de lote considerados "Pendientes" (todo lo anterior a Enviado).
        /// </summary>
        private const string ESTADOS_PENDIENTES = "1,2,3,4,5,6,7,8,9,10";

        /// <summary>
        /// Estado de lote "Enviado".
        /// </summary>
        private const int CD_ESTADO_ENVIADO = 11;

        public EstadoXEntregaDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene el resumen de lotes y planos por entrega (grillas 1 y 2 de
        /// FrmEstadoXEntrega.cs).
        /// </summary>
        public List<EstadoXEntregaDto> ObtenerResumenPorEntrega()
        {
            var lista = new List<EstadoXEntregaDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand($@"
                    SELECT      ap.dsEntrega,
                                COUNT(DISTINCT l.cdLote)                                                              AS nuTotalLotes,
                                COUNT(DISTINCT CASE WHEN l.cdEstadoLote IN ({ESTADOS_PENDIENTES}) THEN l.cdLote END)   AS nuLotesPendientes,
                                COUNT(DISTINCT CASE WHEN l.cdEstadoLote = @cdEstadoEnviado THEN l.cdLote END)         AS nuLotesEnviados,
                                COUNT(ap.cdArchivoPagina)                                                             AS nuTotalPlanos,
                                COUNT(CASE WHEN l.cdEstadoLote IN ({ESTADOS_PENDIENTES}) THEN ap.cdArchivoPagina END)  AS nuPlanosPendientes,
                                COUNT(CASE WHEN l.cdEstadoLote = @cdEstadoEnviado THEN ap.cdArchivoPagina END)         AS nuPlanosEnviados
                    FROM        TD_ARCHIVOS_PAGINAS ap
                    LEFT JOIN   TD_LOTE_ARCHIVOS la ON la.cdArchivoPagina = ap.cdArchivoPagina
                    LEFT JOIN   TD_LOTE l ON l.cdLote = la.cdLote
                    GROUP BY    ap.dsEntrega
                    ORDER BY    ap.dsEntrega", conexion);

                comando.Parameters.AddWithValue("@cdEstadoEnviado", CD_ESTADO_ENVIADO);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new EstadoXEntregaDto
                        {
                            DsEntrega = lector.IsDBNull(0) ? string.Empty : lector.GetString(0),
                            NuTotalLotes = lector.GetInt32(1),
                            NuLotesPendientes = lector.GetInt32(2),
                            NuLotesEnviados = lector.GetInt32(3),
                            NuTotalPlanos = lector.GetInt32(4),
                            NuPlanosPendientes = lector.GetInt32(5),
                            NuPlanosEnviados = lector.GetInt32(6)
                        });
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Obtiene, para la entrega indicada, la cantidad de lotes pendientes
        /// (cdEstadoLote IN 1..10) agrupados por estado de lote.
        /// </summary>
        public List<LoteEstadoAgrupadoDto> ObtenerLotesPendientesPorEstado(string dsEntrega)
        {
            var lista = new List<LoteEstadoAgrupadoDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand($@"
                    SELECT      l.cdEstadoLote,
                                e.dsEstado,
                                COUNT(DISTINCT l.cdLote) AS nuCantidadLotes
                    FROM        TD_LOTE l
                    INNER JOIN  TD_LOTE_ARCHIVOS la ON la.cdLote = l.cdLote
                    INNER JOIN  TD_ARCHIVOS_PAGINAS ap ON ap.cdArchivoPagina = la.cdArchivoPagina
                    LEFT JOIN   TD_ESTADOS e ON e.dsProceso = 'LOTE' AND e.cdEstado = l.cdEstadoLote
                    WHERE       ap.dsEntrega = @dsEntrega
                                AND l.cdEstadoLote IN ({ESTADOS_PENDIENTES})
                    GROUP BY    l.cdEstadoLote, e.dsEstado
                    ORDER BY    l.cdEstadoLote", conexion);

                comando.Parameters.AddWithValue("@dsEntrega", dsEntrega);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new LoteEstadoAgrupadoDto
                        {
                            CdEstadoLote = lector.GetInt32(0),
                            DsEstadoLote = lector.IsDBNull(1) ? string.Empty : lector.GetString(1),
                            NuCantidadLotes = lector.GetInt32(2)
                        });
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Obtiene el detalle de lotes filtrado por entrega y estado de lote seleccionado,
        /// usado en la segunda grilla de FrmLotesPendientesEntrega.
        /// </summary>
        public List<LoteDetalleEstadoDto> ObtenerDetalleLotesPorEntregaYEstado(string dsEntrega, int cdEstadoLote)
        {
            var lista = new List<LoteDetalleEstadoDto>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    SELECT DISTINCT
                                a.cdLote,
                                b.dsEstado                 AS dsEstadoLote,
                                a.nuCantidadArchivos,

                                a.cdUsuarioAsignado         AS cdUsuarioAsignadoControl,
                                c.dsNombreCompleto          AS dsUsuarioAsignadoControl,
                                a.feFinControl              AS feFinControl,
                                a.cdUsuarioFinControl       AS cdUsuarioControl,
                                d.dsNombreCompleto          AS dsUsuarioControl,

                                a.cdUsuarioAuditor          AS cdUsuarioAsignadoAuditoria,
                                e.dsNombreCompleto          AS dsUsuarioAsignadoAuditoria,
                                a.feFinAuditoria            AS feFinAuditoria,
                                a.cdUsuarioAuditado         AS cdUsuarioAuditor,
                                f.dsNombreCompleto          AS dsUsuarioAuditor,

                                a.feFinalizado              AS feFinalizado,
                                a.cdUsuarioFinalizado       AS cdUsuarioFinalizado,
                                g.dsUsuario                 AS dsUsuarioFinalizado
                    FROM        TD_LOTE         a
                    INNER JOIN  TD_LOTE_ARCHIVOS la ON la.cdLote = a.cdLote
                    INNER JOIN  TD_ARCHIVOS_PAGINAS ap ON ap.cdArchivoPagina = la.cdArchivoPagina
                    LEFT JOIN   TD_ESTADOS     b ON b.dsProceso = 'LOTE' AND a.cdEstadoLote = b.cdEstado
                    LEFT JOIN   TD_USUARIOS    c ON a.cdUsuarioAsignado = c.cdUsuario
                    LEFT JOIN   TD_USUARIOS    d ON a.cdUsuarioFinControl = d.cdUsuario
                    LEFT JOIN   TD_USUARIOS    e ON a.cdUsuarioAuditor = e.cdUsuario
                    LEFT JOIN   TD_USUARIOS    f ON a.cdUsuarioAuditado = f.cdUsuario
                    LEFT JOIN   TD_USUARIOS    g ON a.cdUsuarioFinalizado = g.cdUsuario
                    WHERE       ap.dsEntrega = @dsEntrega
                                AND a.cdEstadoLote = @cdEstadoLote
                    ORDER BY    a.cdLote", conexion);

                comando.Parameters.AddWithValue("@dsEntrega", dsEntrega);
                comando.Parameters.AddWithValue("@cdEstadoLote", cdEstadoLote);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new LoteDetalleEstadoDto
                        {
                            CdLote = lector.GetInt32(0),
                            DsEstadoLote = lector.IsDBNull(1) ? null : lector.GetString(1),
                            NuCantidadArchivos = lector.GetInt32(2),

                            CdUsuarioAsignadoControl = lector.IsDBNull(3) ? null : lector.GetInt32(3),
                            DsUsuarioAsignadoControl = lector.IsDBNull(4) ? null : lector.GetString(4),
                            FeFinControl = lector.IsDBNull(5) ? null : lector.GetDateTime(5),
                            CdUsuarioControl = lector.IsDBNull(6) ? null : lector.GetInt32(6),
                            DsUsuarioControl = lector.IsDBNull(7) ? null : lector.GetString(7),

                            CdUsuarioAsignadoAuditoria = lector.IsDBNull(8) ? null : lector.GetInt32(8),
                            DsUsuarioAsignadoAuditoria = lector.IsDBNull(9) ? null : lector.GetString(9),
                            FeFinAuditoria = lector.IsDBNull(10) ? null : lector.GetDateTime(10),
                            CdUsuarioAuditor = lector.IsDBNull(11) ? null : lector.GetInt32(11),
                            DsUsuarioAuditor = lector.IsDBNull(12) ? null : lector.GetString(12),

                            FeFinalizado = lector.IsDBNull(13) ? null : lector.GetDateTime(13),
                            CdUsuarioFinalizado = lector.IsDBNull(14) ? null : lector.GetInt32(14),
                            DsUsuarioFinalizado = lector.IsDBNull(15) ? null : lector.GetString(15)
                        });
                    }
                }
            }

            return lista;
        }
    }
}
