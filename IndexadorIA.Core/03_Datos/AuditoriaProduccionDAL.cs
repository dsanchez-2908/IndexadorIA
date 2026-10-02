using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para el reporte "Auditoria x Usuario": resume, por
    /// usuario auditor y rango de fechas de auditoria, la cantidad de lotes y de
    /// planos (archivos) auditados.
    /// </summary>
    public class AuditoriaProduccionDAL
    {
        private readonly string _cadenaConexion;

        public AuditoriaProduccionDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene la cantidad de lotes y planos auditados por usuario en el rango
        /// de fechas indicado (TD_LOTE.feAuditado).
        /// Si <paramref name="cdUsuario"/> es null o 0, se incluyen todos los usuarios
        /// con al menos un lote auditado en el rango.
        /// Si <paramref name="detallePorFecha"/> es true, el resultado se desagrega por
        /// dia (una fila por usuario y fecha); si es false, se totaliza por usuario.
        /// </summary>
        public List<AuditoriaUsuarioDto> ObtenerAuditoriaPorUsuario(int? cdUsuario, DateTime feDesde, DateTime feHasta, bool detallePorFecha = false)
        {
            var lista = new List<AuditoriaUsuarioDto>();
            int cdUsuarioFiltro = cdUsuario ?? 0;

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                string sql = detallePorFecha
                    ? @"
                    SELECT
                        b.cdUsuario,
                        b.dsUsuario,
                        CAST(a.feAuditado AS DATE) AS Fecha,
                        COUNT(DISTINCT a.cdLote) AS CantidadLotes,
                        SUM(a.nuCantidadArchivos) AS CantidadPlanos
                    FROM TD_LOTE a
                    INNER JOIN TD_USUARIOS b ON a.cdUsuarioAuditor = b.cdUsuario
                    WHERE a.cdUsuarioAuditor IS NOT NULL
                      AND a.feAuditado BETWEEN @feDesde AND @feHasta
                      AND (@cdUsuario = 0 OR a.cdUsuarioAuditor = @cdUsuario)
                    GROUP BY b.cdUsuario, b.dsUsuario, CAST(a.feAuditado AS DATE)
                    ORDER BY b.dsUsuario, Fecha"
                    : @"
                    SELECT
                        b.cdUsuario,
                        b.dsUsuario,
                        NULL AS Fecha,
                        COUNT(DISTINCT a.cdLote) AS CantidadLotes,
                        SUM(a.nuCantidadArchivos) AS CantidadPlanos
                    FROM TD_LOTE a
                    INNER JOIN TD_USUARIOS b ON a.cdUsuarioAuditor = b.cdUsuario
                    WHERE a.cdUsuarioAuditor IS NOT NULL
                      AND a.feAuditado BETWEEN @feDesde AND @feHasta
                      AND (@cdUsuario = 0 OR a.cdUsuarioAuditor = @cdUsuario)
                    GROUP BY b.cdUsuario, b.dsUsuario
                    ORDER BY b.dsUsuario";

                var comando = new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue("@cdUsuario", cdUsuarioFiltro);
                comando.Parameters.AddWithValue("@feDesde", feDesde);
                comando.Parameters.AddWithValue("@feHasta", feHasta);

                conexion.Open();
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new AuditoriaUsuarioDto
                        {
                            CdUsuario = reader.GetInt32(0),
                            DsUsuario = reader.GetString(1),
                            Fecha = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            CantidadLotes = reader.GetInt32(3),
                            CantidadPlanos = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                        });
                    }
                }
            }

            return lista;
        }
    }
}
