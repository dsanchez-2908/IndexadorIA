namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un archivo original (PDF o imagen)
    /// </summary>
    public class Archivo
    {
        public int CdArchivo { get; set; }
        public int CdProyecto { get; set; }
        public string DsNombreOriginal { get; set; } = string.Empty;
        public string DsRutaCompleta { get; set; } = string.Empty;
        public string DsTipoArchivo { get; set; } = string.Empty; // PDF, JPG, JPEG
        public long NuTamanoBytes { get; set; }
        public int NuPaginasTotal { get; set; } = 1;
        public int CdEstado { get; set; }
        public DateTime FeIngreso { get; set; }
        public int CdUsuarioIngreso { get; set; }
        public DateTime? FeProcesamiento { get; set; }
        public string? DsObservaciones { get; set; }
    }
}
