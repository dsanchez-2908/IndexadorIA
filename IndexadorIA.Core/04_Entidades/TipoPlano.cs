namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un tipo de plano (perteneciente a una categoría:
    /// OBRAS, CATASTRO o INSTALACIONES)
    /// </summary>
    public class TipoPlano
    {
        public int CdTipoPlano { get; set; }
        public int CdCategoriaPlano { get; set; }
        public string DsTipoPlano { get; set; } = string.Empty;
        public bool SnActivo { get; set; }
        public string? DsAcronimo { get; set; }
        public DateTime FeAlta { get; set; }

        // Navegación (opcional)
        public string? DsCategoriaPlano { get; set; }
    }
}
