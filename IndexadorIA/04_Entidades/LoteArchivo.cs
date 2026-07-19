namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Representa la relación entre un lote y un archivo página
    /// </summary>
    public class LoteArchivo
    {
        public int Id { get; set; }
        public int CdLote { get; set; }
        public int CdArchivoPagina { get; set; }
    }
}
