namespace IndexadorIA.Entidades
{
    public class ArchivoPagina
    {
        public int CdArchivoPagina { get; set; }
        public int CdArchivoOriginal { get; set; }
        public int NuPagina { get; set; }
        public string DsNombreArchivoPagina { get; set; } = string.Empty;
        public string DsRutaCompleta { get; set; } = string.Empty;
        public string SnGirada { get; set; } = "NO";
        public string SnPosibleBlanca { get; set; } = "NO";
        public string DsProceso { get; set; } = "ARCHIVO_PAGINA";
        public int CdEstado { get; set; }
        public DateTime FeAlta { get; set; }
        public int CdUsuarioAlta { get; set; }

        // Propiedades adicionales para UI
        public string DsArchivoOriginal { get; set; } = string.Empty;
        public string DsEstado { get; set; } = string.Empty;
        public string? NombreArchivo { get; set; }
        public string? RutaBase64 { get; set; }

        /// <summary>
        /// Nombre final del PDF (ya renombrado/movido durante la finalización del lote),
        /// persistido en TD_ARCHIVOS_PAGINAS.dsNombreArchivoFinal.
        /// </summary>
        public string? DsNombreArchivoFinal { get; set; }

        /// <summary>
        /// Ruta completa (dsRutaCompleta) del archivo original en TD_ARCHIVOS_ORIGINAL,
        /// usada para calcular la carpeta donde se ubican los PDFs finales
        /// ("Planos Procesados"/"Planos ILEGIBLES").
        /// </summary>
        public string DsRutaCompletaOriginal { get; set; } = string.Empty;
    }
}
