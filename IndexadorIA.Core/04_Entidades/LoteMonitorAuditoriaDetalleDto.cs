namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO de detalle de un lote para la pantalla de detalle del Monitor de Auditoria:
    /// incluye el estado del lote y la cantidad de planos.
    /// </summary>
    public class LoteMonitorAuditoriaDetalleDto
    {
        public bool SnSeleccionado { get; set; }
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int CdEstadoLote { get; set; }
        public string DsEstadoLote { get; set; } = string.Empty;

        public int NuCantidadPlanos { get; set; }
    }
}
