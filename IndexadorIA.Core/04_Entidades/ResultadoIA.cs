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
        public int? CdCategoriaPlano { get; set; }
        public int? CdTipoPlano { get; set; }
        public string? DsExpediente { get; set; }
        public string? DsSeccion { get; set; }
        public string? DsManzana { get; set; }
        public string? DsParcela { get; set; }
        public string? DsDireccion { get; set; }
        public string? DsNumeroPlano { get; set; }

        // Niveles de confianza
        public decimal? NuConfianzaCategoriaPlano { get; set; }
        public decimal? NuConfianzaTipoPlano { get; set; }
        public decimal? NuConfianzaExpediente { get; set; }
        public decimal? NuConfianzaSeccion { get; set; }
        public decimal? NuConfianzaManzana { get; set; }
        public decimal? NuConfianzaParcela { get; set; }
        public decimal? NuConfianzaDireccion { get; set; }
        public decimal? NuConfianzaNumeroPlano { get; set; }

        // Auditoría
        public DateTime FeAlta { get; set; }
        public int CdUsuarioAlta { get; set; }
        public DateTime? FeUltimaModificacion { get; set; }
        public int? CdUsuarioModificacion { get; set; }

        // Control de calidad
        public int CdEstadoControl { get; set; }
        public string? SnModificaDatos { get; set; }
        public DateTime? FeControl { get; set; }
        public int? CdUsuarioControl { get; set; }

        // Observaciones opcionales ingresadas manualmente por el usuario
        public string? DsObservaciones { get; set; }

        // Navegación (opcional)
        public string? DsTipoPlano { get; set; }
        public string? DsCategoriaPlano { get; set; }
        public string? NombreArchivo { get; set; }
        public int? NumeroPagina { get; set; }
        public string? DsEstadoControl { get; set; }

        /// <summary>
        /// Estados posibles para el proceso de control (dsProceso = 'CONTROL' en TD_ESTADOS)
        /// </summary>
        public static class EstadosControl
        {
            public const int PendienteControl = 1;
            public const int Controlado = 2;
            public const int PaginaIlegible = 3;
            public const int DatosIlegibles = 4;
        }
    }
}
