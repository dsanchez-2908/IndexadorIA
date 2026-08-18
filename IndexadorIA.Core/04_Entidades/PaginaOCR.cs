namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad para los resultados de OCR de páginas
    /// </summary>
    public class PaginaOCR
    {
        public int Id { get; set; }
        public int CdArchivoPagina { get; set; }
        public string? TxtResultadoOCR { get; set; }
        public DateTime FeOCR { get; set; }
    }
}
