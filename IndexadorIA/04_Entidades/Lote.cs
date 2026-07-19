namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un lote de archivos página para procesar en batch
    /// </summary>
    public class Lote
    {
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int NuCantidadArchivos { get; set; }
        public int CdEstadoLote { get; set; }
        public DateTime FeAltaLote { get; set; }
        public int? CdUsuarioAltaLote { get; set; }

        // Propiedades navegación/display
        public string? DsEstado { get; set; }
        public string? DsUsuario { get; set; }
    }
}
