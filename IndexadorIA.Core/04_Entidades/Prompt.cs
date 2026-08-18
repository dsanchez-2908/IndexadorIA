namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un prompt configurado para un proyecto
    /// </summary>
    public class Prompt
    {
        public int CdPrompt { get; set; }
        public int CdProyecto { get; set; }
        public string? DsDescripcion { get; set; }
        public string DsPrompt { get; set; } = string.Empty;
        public DateTime FeAlta { get; set; }
        public int CdUsuarioAlta { get; set; }
        public DateTime? FeUltimaModificacion { get; set; }
        public int? CdUsuarioModificacion { get; set; }
    }
}
