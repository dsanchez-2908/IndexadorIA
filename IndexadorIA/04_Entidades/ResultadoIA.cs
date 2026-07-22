namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa el resultado del análisis de IA para un archivo/página
    /// </summary>
    public class ResultadoIA
    {
        public int CdResultado { get; set; }
        public int CdLote { get; set; }
        public int CdArchivoPagina { get; set; }

        // Datos extraídos
        public int? CdTipoPlano { get; set; }
        public string? DsExpediente { get; set; }
        public string? DsSeccion { get; set; }
        public string? DsManzana { get; set; }
        public string? DsParcela { get; set; }
        public string? DsDireccion { get; set; }

        // Niveles de confianza
        public decimal? NuConfianzaTipoPlano { get; set; }
        public decimal? NuConfianzaExpediente { get; set; }
        public decimal? NuConfianzaSeccion { get; set; }
        public decimal? NuConfianzaManzana { get; set; }
        public decimal? NuConfianzaParcela { get; set; }
        public decimal? NuConfianzaDireccion { get; set; }

        // Auditoría
        public DateTime FeAlta { get; set; }
        public int CdUsuarioAlta { get; set; }
        public DateTime? FeUltimaModificacion { get; set; }
        public int? CdUsuarioModificacion { get; set; }

        // Navegación (opcional)
        public string? DsTipoPlano { get; set; }
        public string? NombreArchivo { get; set; }
        public int? NumeroPagina { get; set; }
    }
}
