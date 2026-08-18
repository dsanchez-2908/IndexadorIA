namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un proyecto de indexación
    /// </summary>
    public class Proyecto
    {
        public int CdProyecto { get; set; }
        public string DsProyecto { get; set; } = string.Empty;
        public bool SnActivo { get; set; }
        public DateTime FeAlta { get; set; }
        public int CdUsuarioAlta { get; set; }
        public DateTime? FeUltimaModificacion { get; set; }
        public int? CdUsuarioModificacion { get; set; }
    }
}
