using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para el reporte "Estado Proyecto Planos": resume el
    /// avance general de digitalización, control, auditoría, finalización y envío
    /// de planos del proyecto de GCBA (cdProyecto = 1), organizado por el flujo de
    /// estados de lote (TD_LOTE.cdEstadoLote) y estados de control (cdEstadoControl).
    /// </summary>
    public class EstadoProyectoDAL
    {
        private readonly string _cadenaConexion;

        public EstadoProyectoDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene los indicadores del estado general del proyecto indicado.
        /// </summary>
        public EstadoProyectoDto ObtenerEstadoProyecto(int cdProyecto)
        {
            var dto = new EstadoProyectoDto();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                // ===================== Grupo: Total de Proyecto Planos de GCBA =====================
                using (var cmd = new SqlCommand(
                    "SELECT ISNULL(nuTotalArchivos, 0), ISNULL(nuTotalPlanos, 0) FROM TD_PROYECTOS WHERE cdProyecto = @cdProyecto",
                    conexion))
                {
                    cmd.Parameters.AddWithValue("@cdProyecto", cdProyecto);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dto.TotalArchivosDelProyecto = reader.GetInt32(0);
                            dto.TotalPlanosDelProyecto = reader.GetInt32(1);
                        }
                    }
                }

                // ===================== Grupo: Ingreso al sistema =====================
                dto.TotalArchivosIngresados = EjecutarEscalar(conexion, "SELECT COUNT(*) FROM TD_ARCHIVOS_ORIGINAL");
                dto.TotalPlanosIngresados = EjecutarEscalar(conexion, "SELECT COUNT(*) FROM TD_ARCHIVOS_PAGINAS");

                dto.PendienteIngresarArchivos = dto.TotalArchivosDelProyecto - dto.TotalArchivosIngresados;
                dto.PendienteIngresarPlanos = dto.TotalPlanosDelProyecto - dto.TotalPlanosIngresados;

                // ===================== Grupo: Control de Planos =====================
                dto.TotalLotesPendientesControl = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote IN (4,6)");
                dto.TotalPlanosPendientesControl = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (4,6))");

                dto.TotalLotesAsignadosControl = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote IN (6)");
                dto.TotalPlanosAsignadosControl = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (6))");

                dto.TotalLotesPendientesAsignarControl = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote IN (4)");
                dto.TotalPlanosPendientesAsignarControl = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (4))");

                // ===================== Grupo: Auditoria =====================
                // Sub Grupo: Pendiente de Auditar (7,8)
                dto.TotalLotesPendientesAuditar = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote IN (7,8)");
                dto.TotalPlanosPendientesAuditar = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7,8))");
                dto.PendientesAuditarOk = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7,8)) AND cdEstadoControl IN (2)");
                dto.PendientesAuditarDatosIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7,8)) AND cdEstadoControl IN (4)");
                dto.PendientesAuditarPaginasIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7,8)) AND cdEstadoControl IN (3)");
                dto.PendientesAuditarCasosEspeciales = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7,8)) AND cdEstadoControl IN (5)");
                dto.PendientesAuditarParcelasMultiples = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7,8)) AND cdEstadoControl IN (6)");

                // Sub Grupo: Auditando (Asignados) (8)
                dto.TotalLotesAuditando = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote IN (8)");
                dto.TotalPlanosAuditando = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (8))");
                dto.AuditandoOk = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (8)) AND cdEstadoControl IN (2)");
                dto.AuditandoDatosIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (8)) AND cdEstadoControl IN (4)");
                dto.AuditandoPaginasIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (8)) AND cdEstadoControl IN (3)");
                dto.AuditandoCasosEspeciales = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (8)) AND cdEstadoControl IN (5)");
                dto.AuditandoParcelasMultiples = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (8)) AND cdEstadoControl IN (6)");

                // Sub Grupo: Sin asignar Auditoria (7)
                dto.TotalLotesSinAsignarAuditoria = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote IN (7)");
                dto.TotalPlanosSinAsignarAuditoria = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7))");
                dto.SinAsignarAuditoriaOk = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7)) AND cdEstadoControl IN (2)");
                dto.SinAsignarAuditoriaDatosIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7)) AND cdEstadoControl IN (4)");
                dto.SinAsignarAuditoriaPaginasIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7)) AND cdEstadoControl IN (3)");
                dto.SinAsignarAuditoriaCasosEspeciales = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7)) AND cdEstadoControl IN (5)");
                dto.SinAsignarAuditoriaParcelasMultiples = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (7)) AND cdEstadoControl IN (6)");

                // ===================== Grupo: Planos pendientes de Finalizar (9) =====================
                dto.TotalLotesPendientesFinalizar = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote IN (9)");
                dto.TotalPlanosPendientesFinalizar = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (9))");
                dto.PendientesFinalizarOk = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (9)) AND cdEstadoControl IN (2)");
                dto.PendientesFinalizarDatosIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (9)) AND cdEstadoControl IN (4)");
                dto.PendientesFinalizarPaginasIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (9)) AND cdEstadoControl IN (3)");
                dto.PendientesFinalizarCasosEspeciales = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (9)) AND cdEstadoControl IN (5)");
                dto.PendientesFinalizarParcelasMultiples = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (9)) AND cdEstadoControl IN (6)");

                // ===================== Grupo: Planos Finalizados (Listo para enviar) (10) =====================
                dto.TotalLotesFinalizados = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote IN (10)");
                dto.TotalPlanosFinalizados = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (10))");
                dto.FinalizadosOk = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (10)) AND cdEstadoControl IN (2)");
                dto.FinalizadosDatosIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (10)) AND cdEstadoControl IN (4)");
                dto.FinalizadosPaginasIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (10)) AND cdEstadoControl IN (3)");
                dto.FinalizadosCasosEspeciales = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (10)) AND cdEstadoControl IN (5)");
                dto.FinalizadosParcelasMultiples = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (10)) AND cdEstadoControl IN (6)");

                // ===================== Grupo: Planos Enviados (11) =====================
                dto.TotalLotesEnviados = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_LOTE WHERE cdEstadoLote IN (11)");
                dto.TotalPlanosEnviados = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (11))");
                dto.EnviadosOk = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (11)) AND cdEstadoControl IN (2)");
                dto.EnviadosDatosIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (11)) AND cdEstadoControl IN (4)");
                dto.EnviadosPaginasIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (11)) AND cdEstadoControl IN (3)");
                dto.EnviadosCasosEspeciales = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (11)) AND cdEstadoControl IN (5)");
                dto.EnviadosParcelasMultiples = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdLote IN (SELECT cdLote FROM TD_LOTE WHERE cdEstadoLote IN (11)) AND cdEstadoControl IN (6)");
            }

            // ===================== Grupo: Estado Global (gráficos) =====================
            dto.EstadoGlobalPendienteProyecto = Math.Max(dto.TotalPlanosDelProyecto - dto.TotalPlanosEnviados, 0);
            dto.EstadoGlobalEnviadoProyecto = dto.TotalPlanosEnviados;

            dto.EstadoGlobalPendienteIngresado = Math.Max(dto.TotalPlanosIngresados - dto.TotalPlanosEnviados, 0);
            dto.EstadoGlobalEnviadoIngresado = dto.TotalPlanosEnviados;

            return dto;
        }

        private static int EjecutarEscalar(SqlConnection conexion, string sql)
        {
            using (var cmd = new SqlCommand(sql, conexion))
            {
                var resultado = cmd.ExecuteScalar();
                return resultado == null || resultado == DBNull.Value ? 0 : Convert.ToInt32(resultado);
            }
        }
    }
}
