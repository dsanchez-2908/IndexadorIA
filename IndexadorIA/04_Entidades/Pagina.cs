namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa una página extraída y procesada
    /// </summary>
    public class Pagina
    {
        public int CdPagina { get; set; }
        public int CdArchivo { get; set; }
        public int NuNumeroPagina { get; set; }
        public string DsRutaImagenOriginal { get; set; } = string.Empty;
        public string? DsRutaImagenRotada { get; set; }
        public string? DsRutaImagenRecortada { get; set; }
        public int NuGradosRotacion { get; set; } = 0;
        public bool SnRotada { get; set; } = false;
        public bool SnRecortada { get; set; } = false;
        public int? NuAnchoOriginal { get; set; }
        public int? NuAltoOriginal { get; set; }
        public int CdEstado { get; set; }
        public DateTime FeCreacion { get; set; }
        public DateTime? FeProcesamiento { get; set; }
        public string? DsObservaciones { get; set; }
    }
}
