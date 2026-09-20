using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para el reporte "Planos para enviar": resume los
    /// lotes pendientes de finalizar (cdEstadoLote = 9), los lotes finalizados
    /// listos para enviar (cdEstadoLote = 10) y el total de casos especiales
    /// en toda la base.
    /// </summary>
    public class PlanosParaEnviarDAL
    {
        private const int EstadoLotePendienteFinalizar = 9;
        private const int EstadoLoteFinalizadoParaEnviar = 10;

        private readonly string _cadenaConexion;

        public PlanosParaEnviarDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene los indicadores del reporte "Planos para enviar".
        /// </summary>
        public PlanosParaEnviarDto ObtenerPlanosParaEnviar()
        {
            var dto = new PlanosParaEnviarDto();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                dto.TotalLotesPendienteFinalizar = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote = @cdEstadoLote",
                    EstadoLotePendienteFinalizar);

                dto.TotalPlanosCorrectosPendienteFinalizar = EjecutarEscalar(conexion,
                    @"SELECT COUNT(*) FROM TD_001_RESULTADO_IA
                      WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote = @cdEstadoLote)
                      AND cdEstadoControl = 2",
                    EstadoLotePendienteFinalizar);

                dto.TotalPlanosDatosIlegiblesPendienteFinalizar = EjecutarEscalar(conexion,
                    @"SELECT COUNT(*) FROM TD_001_RESULTADO_IA
                      WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote = @cdEstadoLote)
                      AND cdEstadoControl = 4",
                    EstadoLotePendienteFinalizar);

                dto.TotalPlanosIlegiblesPendienteFinalizar = EjecutarEscalar(conexion,
                    @"SELECT COUNT(*) FROM TD_001_RESULTADO_IA
                      WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote = @cdEstadoLote)
                      AND cdEstadoControl = 3",
                    EstadoLotePendienteFinalizar);

                dto.TotalPlanosCasosEspecialesPendienteFinalizar = EjecutarEscalar(conexion,
                    @"SELECT COUNT(*) FROM TD_001_RESULTADO_IA
                      WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote = @cdEstadoLote)
                      AND cdEstadoControl = 5",
                    EstadoLotePendienteFinalizar);

                dto.TotalLotesFinalizadosParaEnviar = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote = @cdEstadoLote",
                    EstadoLoteFinalizadoParaEnviar);

                dto.TotalPlanosCorrectosFinalizados = EjecutarEscalar(conexion,
                    @"SELECT COUNT(*) FROM TD_001_RESULTADO_IA
                      WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote = @cdEstadoLote)
                      AND cdEstadoControl = 2",
                    EstadoLoteFinalizadoParaEnviar);

                dto.TotalPlanosDatosIlegiblesFinalizados = EjecutarEscalar(conexion,
                    @"SELECT COUNT(*) FROM TD_001_RESULTADO_IA
                      WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote = @cdEstadoLote)
                      AND cdEstadoControl = 4",
                    EstadoLoteFinalizadoParaEnviar);

                dto.TotalPlanosIlegiblesFinalizados = EjecutarEscalar(conexion,
                    @"SELECT COUNT(*) FROM TD_001_RESULTADO_IA
                      WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote = @cdEstadoLote)
                      AND cdEstadoControl = 3",
                    EstadoLoteFinalizadoParaEnviar);

                dto.TotalCasosEspecialesEnBase = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdEstadoControl = 5");
            }

            return dto;
        }

        private static int EjecutarEscalar(SqlConnection conexion, string sql, int? cdEstadoLote = null)
        {
            using (var cmd = new SqlCommand(sql, conexion))
            {
                if (cdEstadoLote.HasValue)
                {
                    cmd.Parameters.AddWithValue("@cdEstadoLote", cdEstadoLote.Value);
                }

                var resultado = cmd.ExecuteScalar();
                return resultado == null || resultado == DBNull.Value ? 0 : Convert.ToInt32(resultado);
            }
        }
    }
}
