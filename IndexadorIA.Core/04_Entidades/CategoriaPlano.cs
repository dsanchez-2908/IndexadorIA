namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa una categoría de plano (OBRAS, CATASTRO, INSTALACIONES)
    /// </summary>
    public class CategoriaPlano
    {
        public int CdCategoriaPlano { get; set; }
        public string DsCategoriaPlano { get; set; } = string.Empty;
        public bool SnActivo { get; set; }
        public string? DsDescripcion { get; set; }
        public DateTime FeAlta { get; set; }
    }
}
