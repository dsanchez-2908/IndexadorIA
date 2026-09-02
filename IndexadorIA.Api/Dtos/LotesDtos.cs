using IndexadorIA.Entidades;

namespace IndexadorIA.Api.Dtos
{
    public class PantallaDto
    {
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }

    public class LoteResumenDto
    {
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int NuCantidadArchivos { get; set; }
        public int CdEstadoLote { get; set; }
        public string? DsEstado { get; set; }
        public DateTime FeAltaLote { get; set; }
        public DateTime? FeProcesamientoIA { get; set; }

        public static LoteResumenDto DesdeEntidad(Lote lote) => new()
        {
            CdLote = lote.CdLote,
            DsNombreLote = lote.DsNombreLote,
            NuCantidadArchivos = lote.NuCantidadArchivos,
            CdEstadoLote = lote.CdEstadoLote,
            DsEstado = lote.DsEstado,
            FeAltaLote = lote.FeAltaLote
        };
    }

    public class ResultadoIADto
    {
        public int CdResultado { get; set; }
        public int CdLote { get; set; }
        public int CdArchivoPagina { get; set; }
        public int? CdCategoriaPlano { get; set; }
        public int? CdTipoPlano { get; set; }
        public string? DsNumeroPlano { get; set; }
        public string? DsExpediente { get; set; }
        public string? DsSeccion { get; set; }
        public string? DsManzana { get; set; }
        public string? DsParcela { get; set; }
        public string? DsDireccion { get; set; }
        public int CdEstadoControl { get; set; }
        public string? DsObservaciones { get; set; }
        public decimal? NuConfianzaCategoriaPlano { get; set; }
        public decimal? NuConfianzaTipoPlano { get; set; }
        public decimal? NuConfianzaNumeroPlano { get; set; }
        public decimal? NuConfianzaExpediente { get; set; }
        public decimal? NuConfianzaSeccion { get; set; }
        public decimal? NuConfianzaManzana { get; set; }
        public decimal? NuConfianzaParcela { get; set; }
        public decimal? NuConfianzaDireccion { get; set; }

        public static ResultadoIADto DesdeEntidad(ResultadoIA r) => new()
        {
            CdResultado = r.CdResultado,
            CdLote = r.CdLote,
            CdArchivoPagina = r.CdArchivoPagina,
            CdCategoriaPlano = r.CdCategoriaPlano,
            CdTipoPlano = r.CdTipoPlano,
            DsNumeroPlano = r.DsNumeroPlano,
            DsExpediente = r.DsExpediente,
            DsSeccion = r.DsSeccion,
            DsManzana = r.DsManzana,
            DsParcela = r.DsParcela,
            DsDireccion = r.DsDireccion,
            CdEstadoControl = r.CdEstadoControl,
            DsObservaciones = r.DsObservaciones,
            NuConfianzaCategoriaPlano = r.NuConfianzaCategoriaPlano,
            NuConfianzaTipoPlano = r.NuConfianzaTipoPlano,
            NuConfianzaNumeroPlano = r.NuConfianzaNumeroPlano,
            NuConfianzaExpediente = r.NuConfianzaExpediente,
            NuConfianzaSeccion = r.NuConfianzaSeccion,
            NuConfianzaManzana = r.NuConfianzaManzana,
            NuConfianzaParcela = r.NuConfianzaParcela,
            NuConfianzaDireccion = r.NuConfianzaDireccion
        };
    }

    public class ArchivoPaginaDto
    {
        public int CdArchivoPagina { get; set; }
        public int NuPagina { get; set; }
        public string DsNombreArchivoPagina { get; set; } = string.Empty;
        public string? NombreArchivo { get; set; }
        public string SnGirada { get; set; } = "NO";
        public string SnPosibleBlanca { get; set; } = "NO";

        public static ArchivoPaginaDto DesdeEntidad(ArchivoPagina a) => new()
        {
            CdArchivoPagina = a.CdArchivoPagina,
            NuPagina = a.NuPagina,
            DsNombreArchivoPagina = a.DsNombreArchivoPagina,
            NombreArchivo = a.NombreArchivo ?? a.DsArchivoOriginal,
            SnGirada = a.SnGirada,
            SnPosibleBlanca = a.SnPosibleBlanca
        };
    }

    public class FilaLoteDetalleDto
    {
        public ArchivoPaginaDto Archivo { get; set; } = new();
        public ResultadoIADto? Resultado { get; set; }
    }

    public class ActualizarResultadoRequestDto
    {
        public int? CdCategoriaPlano { get; set; }
        public int? CdTipoPlano { get; set; }
        public string? DsNumeroPlano { get; set; }
        public string? DsExpediente { get; set; }
        public string? DsSeccion { get; set; }
        public string? DsManzana { get; set; }
        public string? DsParcela { get; set; }
        public string? DsDireccion { get; set; }
    }

    public class ActualizarEstadoControlRequestDto
    {
        public int CdEstadoControl { get; set; }
        public string? SnModificaDatos { get; set; }
        public string? DsObservaciones { get; set; }
    }

    public class CorreccionRequestDto
    {
        public string DsCampo { get; set; } = string.Empty;
        public string? DsValorAnterior { get; set; }
        public string? DsValorNuevo { get; set; }
    }

    public class RegistrarCorreccionesRequestDto
    {
        public List<CorreccionRequestDto> Correcciones { get; set; } = new();
    }

    public class CategoriaPlanoDto
    {
        public int CdCategoriaPlano { get; set; }
        public string DsCategoriaPlano { get; set; } = string.Empty;
    }

    public class TipoPlanoDto
    {
        public int CdTipoPlano { get; set; }
        public int CdCategoriaPlano { get; set; }
        public string DsTipoPlano { get; set; } = string.Empty;
    }

    public class ReparticionDto
    {
        public int CdReparticion { get; set; }
        public string DsReparticion { get; set; } = string.Empty;
    }
}
