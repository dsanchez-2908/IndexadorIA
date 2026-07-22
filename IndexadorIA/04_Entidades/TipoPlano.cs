namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un tipo de plano (Obra, Mensura, Instalaciones)
    /// </summary>
    public class TipoPlano
    {
        public int CdTipoPlano { get; set; }
        public string DsTipoPlano { get; set; } = string.Empty;
        public bool SnActivo { get; set; }
        public string? DsDescripcion { get; set; }
        public DateTime FeAlta { get; set; }
    }
}
