namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO usado para mostrar en la grilla de FrmEnvioLote los lotes en estado
    /// "Pendiente de Enviar" (cdEstado=10), con sus estadisticas por estado de control
    /// y la cantidad de correcciones manuales registradas.
    /// </summary>
    public class LoteEnvioGridDto
    {
        public bool SnSeleccionado { get; set; }
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public string? DsDescripcion { get; set; }
        public string? DsEntrega { get; set; }
        public string? DsCarpeta { get; set; }
        public string? DsUsuarioAsignado { get; set; }
        public string? DsUsuarioAuditor { get; set; }
        public int NuCantidadPlanos { get; set; }
        public int NuCantidadControlado { get; set; }
        public int NuCantidadPendiente { get; set; }
        public int NuCantidadPaginaIlegible { get; set; }
        public int NuCantidadDatosIlegibles { get; set; }
        public int NuCantidadCasosEspeciales { get; set; }
        public int NuCantidadParcelasMultiples { get; set; }
        public int NuCantidadCorregido { get; set; }
    }
}
