namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Fila del reporte "Estado x Entrega" (grilla 1: Lotes y grilla 2: Planos) de
    /// FrmEstadoXEntrega. Agrupa los lotes/planos por entrega (TD_ARCHIVOS_PAGINAS.dsEntrega)
    /// indicando total, pendientes (cdEstadoLote IN 1..10) y enviados (cdEstadoLote = 11).
    /// </summary>
    public class EstadoXEntregaDto
    {
        public string DsEntrega { get; set; } = string.Empty;

        public int NuTotalLotes { get; set; }
        public int NuLotesPendientes { get; set; }
        public int NuLotesEnviados { get; set; }

        public int NuTotalPlanos { get; set; }
        public int NuPlanosPendientes { get; set; }
        public int NuPlanosEnviados { get; set; }
    }

    /// <summary>
    /// Fila de la grilla de agrupacion por estado de lote (FrmLotesPendientesEntrega),
    /// para los lotes pendientes (cdEstadoLote IN 1..10) de una entrega seleccionada.
    /// </summary>
    public class LoteEstadoAgrupadoDto
    {
        public int CdEstadoLote { get; set; }
        public string DsEstadoLote { get; set; } = string.Empty;
        public int NuCantidadLotes { get; set; }
    }

    /// <summary>
    /// Fila de detalle de lotes filtrado por entrega y estado de lote seleccionado,
    /// mostrada en la segunda grilla de FrmLotesPendientesEntrega.
    /// </summary>
    public class LoteDetalleEstadoDto
    {
        public int CdLote { get; set; }
        public string? DsEstadoLote { get; set; }
        public int NuCantidadArchivos { get; set; }

        public int? CdUsuarioAsignadoControl { get; set; }
        public string? DsUsuarioAsignadoControl { get; set; }
        public DateTime? FeFinControl { get; set; }
        public int? CdUsuarioControl { get; set; }
        public string? DsUsuarioControl { get; set; }

        public int? CdUsuarioAsignadoAuditoria { get; set; }
        public string? DsUsuarioAsignadoAuditoria { get; set; }
        public DateTime? FeFinAuditoria { get; set; }
        public int? CdUsuarioAuditor { get; set; }
        public string? DsUsuarioAuditor { get; set; }

        public DateTime? FeFinalizado { get; set; }
        public int? CdUsuarioFinalizado { get; set; }
        public string? DsUsuarioFinalizado { get; set; }
    }
}
