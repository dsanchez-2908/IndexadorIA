namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO de detalle de un lote para la pantalla de detalle del Monitor de Lotes:
    /// incluye el estado del lote y las estadísticas de control de sus páginas.
    /// </summary>
    public class LoteMonitorDetalleDto
    {
        public bool SnSeleccionado { get; set; }
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int CdEstadoLote { get; set; }
        public string DsEstadoLote { get; set; } = string.Empty;

        public int NuCantidadPlanos { get; set; }
        public int NuCantidadControlado { get; set; }
        public int NuCantidadPendiente { get; set; }
        public int NuCantidadPaginaIlegible { get; set; }
        public int NuCantidadDatosIlegibles { get; set; }
        public int NuCantidadCorregido { get; set; }
    }
}
