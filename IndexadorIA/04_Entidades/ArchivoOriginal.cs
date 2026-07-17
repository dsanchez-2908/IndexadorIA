namespace IndexadorIA.Entidades
{
    public class ArchivoOriginal
    {
        public int CdArchivo { get; set; }
        public int CdProyecto { get; set; }
        public string DsNombreArchivo { get; set; } = string.Empty;
        public string DsExtension { get; set; } = string.Empty;
        public string DsRutaCompleta { get; set; } = string.Empty;
        public string? DsNombreUltimaCarpeta { get; set; }
        public int NuCantidadPaginas { get; set; }
        public string DsProceso { get; set; } = "ARCHIVO_ORIGINAL";
        public int CdEstadoArchivo { get; set; }
        public DateTime FeAlta { get; set; }
        public int CdUsuarioAlta { get; set; }
        public DateTime? FeUltimaModificacion { get; set; }
        public int? CdUsuarioModificacion { get; set; }
        public long NuTamanoBytes { get; set; }
        public DateTime FeModificacionArchivo { get; set; }

        // Propiedades adicionales para UI
        public string DsProyecto { get; set; } = string.Empty;
        public string DsEstado { get; set; } = string.Empty;
    }
}
