namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO con los indicadores del reporte "Estado Proyecto Planos", organizado en
    /// Grupos (nivel 1), Sub-Grupos (nivel 2) y Detalle (nivel 3) según el flujo de
    /// estados de lote (TD_LOTE.cdEstadoLote) y estados de control (cdEstadoControl).
    /// </summary>
    public class EstadoProyectoDto
    {
        // ===================== Grupo: Total de Proyecto Planos de GCBA =====================
        public int TotalArchivosDelProyecto { get; set; }
        public int TotalPlanosDelProyecto { get; set; }

        // ===================== Grupo: Ingreso al sistema =====================
        // Sub Grupo: Ingresos
        public int TotalArchivosIngresados { get; set; }
        public int TotalPlanosIngresados { get; set; }

        // Sub Grupo: Pendiente Ingresar
        public int PendienteIngresarArchivos { get; set; }
        public int PendienteIngresarPlanos { get; set; }

        // ===================== Grupo: Control de Planos =====================
        // Sub Grupo: Total Pendiente de Control (cdEstadoLote IN (4,6))
        public int TotalLotesPendientesControl { get; set; }
        public int TotalPlanosPendientesControl { get; set; }

        // Sub Grupo: Total Asignados para Control (cdEstadoLote IN (6))
        public int TotalLotesAsignadosControl { get; set; }
        public int TotalPlanosAsignadosControl { get; set; }

        // Sub Grupo: Total Pendiente de Asignar Control (cdEstadoLote IN (4))
        public int TotalLotesPendientesAsignarControl { get; set; }
        public int TotalPlanosPendientesAsignarControl { get; set; }

        // ===================== Grupo: Auditoria =====================
        // Sub Grupo: Pendiente de Auditar (cdEstadoLote IN (7,8))
        public int TotalLotesPendientesAuditar { get; set; }
        public int TotalPlanosPendientesAuditar { get; set; }
        public int PendientesAuditarOk { get; set; }
        public int PendientesAuditarDatosIlegibles { get; set; }
        public int PendientesAuditarPaginasIlegibles { get; set; }
        public int PendientesAuditarCasosEspeciales { get; set; }
        public int PendientesAuditarParcelasMultiples { get; set; }

        // Sub Grupo: Auditando (Asignados) (cdEstadoLote IN (8))
        public int TotalLotesAuditando { get; set; }
        public int TotalPlanosAuditando { get; set; }
        public int AuditandoOk { get; set; }
        public int AuditandoDatosIlegibles { get; set; }
        public int AuditandoPaginasIlegibles { get; set; }
        public int AuditandoCasosEspeciales { get; set; }
        public int AuditandoParcelasMultiples { get; set; }

        // Sub Grupo: Sin asignar Auditoria (cdEstadoLote IN (7))
        public int TotalLotesSinAsignarAuditoria { get; set; }
        public int TotalPlanosSinAsignarAuditoria { get; set; }
        public int SinAsignarAuditoriaOk { get; set; }
        public int SinAsignarAuditoriaDatosIlegibles { get; set; }
        public int SinAsignarAuditoriaPaginasIlegibles { get; set; }
        public int SinAsignarAuditoriaCasosEspeciales { get; set; }
        public int SinAsignarAuditoriaParcelasMultiples { get; set; }

        // ===================== Grupo: Planos pendientes de Finalizar (cdEstadoLote IN (9)) =====================
        public int TotalLotesPendientesFinalizar { get; set; }
        public int TotalPlanosPendientesFinalizar { get; set; }
        public int PendientesFinalizarOk { get; set; }
        public int PendientesFinalizarDatosIlegibles { get; set; }
        public int PendientesFinalizarPaginasIlegibles { get; set; }
        public int PendientesFinalizarCasosEspeciales { get; set; }
        public int PendientesFinalizarParcelasMultiples { get; set; }

        // ===================== Grupo: Planos Finalizados (Listo para enviar) (cdEstadoLote IN (10)) =====================
        public int TotalLotesFinalizados { get; set; }
        public int TotalPlanosFinalizados { get; set; }
        public int FinalizadosOk { get; set; }
        public int FinalizadosDatosIlegibles { get; set; }
        public int FinalizadosPaginasIlegibles { get; set; }
        public int FinalizadosCasosEspeciales { get; set; }
        public int FinalizadosParcelasMultiples { get; set; }

        // ===================== Grupo: Planos Enviados (cdEstadoLote IN (11)) =====================
        public int TotalLotesEnviados { get; set; }
        public int TotalPlanosEnviados { get; set; }
        public int EnviadosOk { get; set; }
        public int EnviadosDatosIlegibles { get; set; }
        public int EnviadosPaginasIlegibles { get; set; }
        public int EnviadosCasosEspeciales { get; set; }
        public int EnviadosParcelasMultiples { get; set; }

        // ===================== Grupo: Estado Global (gráficos) =====================
        // Gráfico 1: Estado Total del Proyecto (Pendiente / Enviado, sobre TotalPlanosDelProyecto)
        public int EstadoGlobalPendienteProyecto { get; set; }
        public int EstadoGlobalEnviadoProyecto { get; set; }

        // Gráfico 2: Estado de Planos (sobre lo ingresado)
        public int EstadoGlobalPendienteIngresado { get; set; }
        public int EstadoGlobalEnviadoIngresado { get; set; }
    }
}
