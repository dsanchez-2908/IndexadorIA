namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO agregado por usuario auditor para la grilla de la pantalla Monitor de Auditoria:
    /// resume la cantidad de lotes y planos asignados/auditados/pendientes de cada auditor.
    /// </summary>
    public class MonitorAuditoriaUsuarioDto
    {
        public int CdUsuario { get; set; }
        public string DsUsuario { get; set; } = string.Empty;

        public int NuTotalLotesAsignados { get; set; }
        public int NuCantidadPlanosAsignados { get; set; }

        public int NuTotalLotesAuditados { get; set; }
        public int NuCantidadPlanosAuditados { get; set; }

        public int NuTotalLotesPendientes { get; set; }
        public int NuCantidadPlanosPendientes { get; set; }
    }
}
