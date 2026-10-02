namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un registro de la vista VW_PLANOS_CONSULTAS,
    /// utilizada por la pantalla de Consulta de Planos.
    /// </summary>
    public class PlanoConsulta
    {
        public int CdResultado { get; set; }
        public int CdLote { get; set; }
        public int? CdEstadoLote { get; set; }
        public string? DsEstadoLote { get; set; }
        public string? DsDescripcionLote { get; set; }
        public string? DsEntrega { get; set; }
        public string? DsCarpeta { get; set; }
        public int? CdEstadoControl { get; set; }
        public string? DsEstadoControl { get; set; }
        public int? CdArchivoPagina { get; set; }
        public int? CdCategoriaPlano { get; set; }
        public string? DsCategoriaPlano { get; set; }
        public int? CdTipoPlano { get; set; }
        public string? DsTipoPlano { get; set; }
        public string? DsExpediente { get; set; }
        public string? DsSeccion { get; set; }
        public string? DsManzana { get; set; }
        public string? DsParcela { get; set; }
        public string? DsDireccion { get; set; }
        public string? DsNumeroPlano { get; set; }
        public string? DsObservaciones { get; set; }
        public string? DsNombreArchivoOriginal { get; set; }
        public string? DsNombreArchivoPagina { get; set; }
        public int? NuPagina { get; set; }
    }

    /// <summary>
    /// Filtros de búsqueda para la Consulta de Planos.
    /// </summary>
    public class PlanoConsultaFiltro
    {
        public int? CdLote { get; set; }
        public int? CdEstadoLote { get; set; }
        public string? DsDescripcionLote { get; set; }
        public string? DsEntrega { get; set; }
        public string? DsCarpeta { get; set; }
        public int? CdEstadoControl { get; set; }
        public int? CdCategoriaPlano { get; set; }
        public int? CdTipoPlano { get; set; }
        public string? DsExpediente { get; set; }
        public string? DsSeccion { get; set; }
        public string? DsManzana { get; set; }
        public string? DsParcela { get; set; }
        public string? DsDireccion { get; set; }
        public string? DsNumeroPlano { get; set; }
        public string? DsObservaciones { get; set; }
        public int MaxRegistros { get; set; } = 10000;
    }
}
