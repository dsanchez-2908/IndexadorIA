using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para el reporte "Producción x Usuarios": resume, por
    /// usuario y rango de fechas, la cantidad de resultados controlados, la cantidad
    /// de lotes finalizados de control y la cantidad de campos corregidos manualmente.
    /// </summary>
    public class ProduccionDAL
    {
        private readonly string _cadenaConexion;

        public ProduccionDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene la producción por usuario en el rango de fechas indicado.
        /// Si <paramref name="cdUsuario"/> es null o 0, se incluyen todos los usuarios
        /// con al menos un dato de producción en el rango.
        /// Si <paramref name="detallePorFecha"/> es true, el resultado se desagrega por
        /// día (una fila por usuario y fecha); si es false, se totaliza por usuario.
        /// </summary>
        public List<ProduccionUsuarioDto> ObtenerProduccionPorUsuario(int? cdUsuario, DateTime feDesde, DateTime feHasta, bool detallePorFecha = false)
        {
            var lista = new List<ProduccionUsuarioDto>();
            int cdUsuarioFiltro = cdUsuario ?? 0;

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                string sql = detallePorFecha
                    ? @"
                    WITH Controlado AS (
                        SELECT cdUsuarioControl, CAST(feControl AS DATE) AS Fecha, COUNT(*) AS Cantidad
                        FROM TD_001_RESULTADO_IA
                        WHERE cdEstadoControl = 2
                          AND feControl BETWEEN @feDesde AND @feHasta
                          AND (@cdUsuario = 0 OR cdUsuarioControl = @cdUsuario)
                        GROUP BY cdUsuarioControl, CAST(feControl AS DATE)
                    ),
                    Correcciones AS (
                        SELECT r.cdUsuarioControl, CAST(r.feControl AS DATE) AS Fecha, COUNT(*) AS Cantidad
                        FROM TD_CORRECIONES c
                        INNER JOIN TD_001_RESULTADO_IA r ON c.cdResultado = r.cdResultado
                        WHERE r.cdEstadoControl = 2
                          AND r.feControl BETWEEN @feDesde AND @feHasta
                          AND (@cdUsuario = 0 OR r.cdUsuarioControl = @cdUsuario)
                        GROUP BY r.cdUsuarioControl, CAST(r.feControl AS DATE)
                    ),
                    LotesCompletos AS (
                        SELECT cdUsuarioFinControl, CAST(feFinControl AS DATE) AS Fecha, COUNT(*) AS Cantidad
                        FROM TD_LOTE
                        WHERE feFinControl BETWEEN @feDesde AND @feHasta
                          AND (@cdUsuario = 0 OR cdUsuarioFinControl = @cdUsuario)
                        GROUP BY cdUsuarioFinControl, CAST(feFinControl AS DATE)
                    ),
                    Fechas AS (
                        SELECT cdUsuarioControl AS cdUsuario, Fecha FROM Controlado
                        UNION
                        SELECT cdUsuarioControl AS cdUsuario, Fecha FROM Correcciones
                        UNION
                        SELECT cdUsuarioFinControl AS cdUsuario, Fecha FROM LotesCompletos
                    )
                    SELECT
                        u.cdUsuario,
                        u.dsNombreCompleto,
                        f.Fecha,
                        ISNULL(co.Cantidad, 0) AS CantidadControlada,
                        ISNULL(l.Cantidad, 0) AS CantidadLotesCompletos,
                        ISNULL(cr.Cantidad, 0) AS CantidadCamposCorregidos
                    FROM Fechas f
                    INNER JOIN TD_USUARIOS u ON u.cdUsuario = f.cdUsuario
                    LEFT JOIN Controlado co ON co.cdUsuarioControl = f.cdUsuario AND co.Fecha = f.Fecha
                    LEFT JOIN Correcciones cr ON cr.cdUsuarioControl = f.cdUsuario AND cr.Fecha = f.Fecha
                    LEFT JOIN LotesCompletos l ON l.cdUsuarioFinControl = f.cdUsuario AND l.Fecha = f.Fecha
                    ORDER BY u.dsNombreCompleto, f.Fecha"
                    : @"
                    WITH Controlado AS (
                        SELECT cdUsuarioControl, COUNT(*) AS Cantidad
                        FROM TD_001_RESULTADO_IA
                        WHERE cdEstadoControl = 2
                          AND feControl BETWEEN @feDesde AND @feHasta
                          AND (@cdUsuario = 0 OR cdUsuarioControl = @cdUsuario)
                        GROUP BY cdUsuarioControl
                    ),
                    Correcciones AS (
                        SELECT r.cdUsuarioControl, COUNT(*) AS Cantidad
                        FROM TD_CORRECIONES c
                        INNER JOIN TD_001_RESULTADO_IA r ON c.cdResultado = r.cdResultado
                        WHERE r.cdEstadoControl = 2
                          AND r.feControl BETWEEN @feDesde AND @feHasta
                          AND (@cdUsuario = 0 OR r.cdUsuarioControl = @cdUsuario)
                        GROUP BY r.cdUsuarioControl
                    ),
                    LotesCompletos AS (
                        SELECT cdUsuarioFinControl, COUNT(*) AS Cantidad
                        FROM TD_LOTE
                        WHERE feFinControl BETWEEN @feDesde AND @feHasta
                          AND (@cdUsuario = 0 OR cdUsuarioFinControl = @cdUsuario)
                        GROUP BY cdUsuarioFinControl
                    )
                    SELECT
                        u.cdUsuario,
                        u.dsNombreCompleto,
                        NULL AS Fecha,
                        ISNULL(co.Cantidad, 0) AS CantidadControlada,
                        ISNULL(l.Cantidad, 0) AS CantidadLotesCompletos,
                        ISNULL(cr.Cantidad, 0) AS CantidadCamposCorregidos
                    FROM TD_USUARIOS u
                    LEFT JOIN Controlado co ON co.cdUsuarioControl = u.cdUsuario
                    LEFT JOIN Correcciones cr ON cr.cdUsuarioControl = u.cdUsuario
                    LEFT JOIN LotesCompletos l ON l.cdUsuarioFinControl = u.cdUsuario
                    WHERE (@cdUsuario = 0 OR u.cdUsuario = @cdUsuario)
                      AND (@cdUsuario <> 0 OR co.Cantidad IS NOT NULL OR l.Cantidad IS NOT NULL OR cr.Cantidad IS NOT NULL)
                    ORDER BY u.dsNombreCompleto";

                var comando = new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue("@cdUsuario", cdUsuarioFiltro);
                comando.Parameters.AddWithValue("@feDesde", feDesde);
                comando.Parameters.AddWithValue("@feHasta", feHasta);

                conexion.Open();
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ProduccionUsuarioDto
                        {
                            CdUsuario = reader.GetInt32(0),
                            DsUsuario = reader.GetString(1),
                            Fecha = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            CantidadControlada = reader.GetInt32(3),
                            CantidadLotesCompletos = reader.GetInt32(4),
                            CantidadCamposCorregidos = reader.GetInt32(5)
                        });
                    }
                }
            }

            return lista;
        }
    }
}
