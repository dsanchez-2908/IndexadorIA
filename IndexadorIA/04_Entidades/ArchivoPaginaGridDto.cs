namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO para mostrar archivos página en la grilla de preparación de lotes
    /// </summary>
    public class ArchivoPaginaGridDto
    {
        public int CdArchivoPagina { get; set; }
        public DateTime FeAlta { get; set; }
        public int CdEstado { get; set; }
        public string DsEstado { get; set; } = string.Empty;
        public string DsNombreArchivoPagina { get; set; } = string.Empty;
        public int NuPagina { get; set; }
        public string DsRutaCompleta { get; set; } = string.Empty;
        public int CdArchivoOriginal { get; set; }
        public int CdProyecto { get; set; }
        public string DsProyecto { get; set; } = string.Empty;
        public string DsNombreArchivo { get; set; } = string.Empty;
        public string DsNombreUltimaCarpeta { get; set; } = string.Empty;
        public int NuCantidadPaginas { get; set; }

        // Propiedad para el checkbox de selección
        public bool Seleccionado { get; set; }
    }
}
