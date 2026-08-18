namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO para la grilla de lotes en la pantalla de preparación de imágenes
    /// </summary>
    public class LoteGridDto
    {
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public DateTime FeAlta { get; set; }
        public int NuCantidadArchivos { get; set; }
        public string DsEstado { get; set; } = string.Empty;
        public bool Seleccionado { get; set; }
    }
}
