using IndexadorIA.Entidades;

namespace IndexadorIA.Api.Dtos
{
    /// <summary>
    /// Registro de VW_PLANOS_CONSULTAS, espejo de PlanoConsulta, usado por la
    /// pantalla de Consulta de Planos en modo remoto.
    /// </summary>
    public class PlanoConsultaDto
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

        public static PlanoConsultaDto DesdeEntidad(PlanoConsulta p) => new()
        {
            CdResultado = p.CdResultado,
            CdLote = p.CdLote,
            CdEstadoLote = p.CdEstadoLote,
            DsEstadoLote = p.DsEstadoLote,
            DsDescripcionLote = p.DsDescripcionLote,
            DsEntrega = p.DsEntrega,
            DsCarpeta = p.DsCarpeta,
            CdEstadoControl = p.CdEstadoControl,
            DsEstadoControl = p.DsEstadoControl,
            CdArchivoPagina = p.CdArchivoPagina,
            CdCategoriaPlano = p.CdCategoriaPlano,
            DsCategoriaPlano = p.DsCategoriaPlano,
            CdTipoPlano = p.CdTipoPlano,
            DsTipoPlano = p.DsTipoPlano,
            DsExpediente = p.DsExpediente,
            DsSeccion = p.DsSeccion,
            DsManzana = p.DsManzana,
            DsParcela = p.DsParcela,
            DsDireccion = p.DsDireccion,
            DsNumeroPlano = p.DsNumeroPlano,
            DsObservaciones = p.DsObservaciones,
            DsNombreArchivoOriginal = p.DsNombreArchivoOriginal,
            DsNombreArchivoPagina = p.DsNombreArchivoPagina,
            NuPagina = p.NuPagina
        };
    }

    /// <summary>
    /// Filtros de busqueda para GET /api/planos-consulta, espejo de PlanoConsultaFiltro.
    /// </summary>
    public class PlanoConsultaFiltroDto
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

    public class EstadoLoteFiltroDto
    {
        public int CdEstado { get; set; }
        public string DsEstado { get; set; } = string.Empty;
    }
}
