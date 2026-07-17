namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un lote de procesamiento
    /// </summary>
    public class Lote
    {
        public int CdLote { get; set; }
        public int CdProyecto { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int NuCantidadPaginas { get; set; } = 0;
        public int CdEstado { get; set; }
        public DateTime FeCreacion { get; set; }
        public int CdUsuarioCreacion { get; set; }
        public DateTime? FeEnvioBatch { get; set; }
        public DateTime? FeFinalizacion { get; set; }
        public string? DsBatchId { get; set; }
        public string? DsResultado { get; set; }
    }
}
