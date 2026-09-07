namespace IndexadorIA.Negocio.Api
{
    /// <summary>
    /// DTOs livianos usados por el cliente WinForms para comunicarse con IndexadorIA.Api,
    /// espejo de los contratos definidos en IndexadorIA.Api.Dtos (sin acoplar el proyecto
    /// WinForms al proyecto Api).
    /// </summary>
    public class LoginApiRequestDto
    {
        public string DsUsuario { get; set; } = string.Empty;
        public string DsClave { get; set; } = string.Empty;
    }

    public class LoginApiResponseDto
    {
        public bool RequiereCambioClave { get; set; }
        public string? Token { get; set; }
        public int CdUsuario { get; set; }
        public string DsUsuario { get; set; } = string.Empty;
        public string DsNombreCompleto { get; set; } = string.Empty;
        public string CdRol { get; set; } = string.Empty;
        public string DsRol { get; set; } = string.Empty;
    }

    public class CambiarClaveApiRequestDto
    {
        public string ClaveActual { get; set; } = string.Empty;
        public string NuevaClave { get; set; } = string.Empty;
        public string ConfirmarClave { get; set; } = string.Empty;
    }

    public class CambiarClaveTemporalApiRequestDto
    {
        public string DsUsuario { get; set; } = string.Empty;
        public string ClaveTemporal { get; set; } = string.Empty;
        public string NuevaClave { get; set; } = string.Empty;
        public string ConfirmarClave { get; set; } = string.Empty;
    }

    public class ApiMensajeDto
    {
        public string? Mensaje { get; set; }
    }

    public class LoteResumenApiDto
    {
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int NuCantidadArchivos { get; set; }
        public int CdEstadoLote { get; set; }
        public string? DsEstado { get; set; }
        public DateTime FeAltaLote { get; set; }
        public DateTime? FeProcesamientoIA { get; set; }
        public int NuControlados { get; set; }
        public int NuPendientes { get; set; }
    }

    public class ResultadoIAApiDto
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
    }

    public class ArchivoPaginaApiDto
    {
        public int CdArchivoPagina { get; set; }
        public int NuPagina { get; set; }
        public string DsNombreArchivoPagina { get; set; } = string.Empty;
        public string? NombreArchivo { get; set; }
        public string SnGirada { get; set; } = "NO";
        public string SnPosibleBlanca { get; set; } = "NO";
    }

    public class FilaLoteDetalleApiDto
    {
        public ArchivoPaginaApiDto Archivo { get; set; } = new();
        public ResultadoIAApiDto? Resultado { get; set; }
    }

    public class LoteDetalleApiDto
    {
        public LoteResumenApiDto Lote { get; set; } = new();
        public List<FilaLoteDetalleApiDto> Filas { get; set; } = new();
    }

    public class ActualizarResultadoApiRequestDto
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

    public class ActualizarEstadoControlApiRequestDto
    {
        public int CdEstadoControl { get; set; }
        public string? SnModificaDatos { get; set; }
        public string? DsObservaciones { get; set; }
    }

    public class CorreccionApiRequestDto
    {
        public string DsCampo { get; set; } = string.Empty;
        public string? DsValorAnterior { get; set; }
        public string? DsValorNuevo { get; set; }
    }

    public class RegistrarCorreccionesApiRequestDto
    {
        public List<CorreccionApiRequestDto> Correcciones { get; set; } = new();
    }

    public class CategoriaPlanoApiDto
    {
        public int CdCategoriaPlano { get; set; }
        public string DsCategoriaPlano { get; set; } = string.Empty;
    }

    public class TipoPlanoApiDto
    {
        public int CdTipoPlano { get; set; }
        public int CdCategoriaPlano { get; set; }
        public string DsTipoPlano { get; set; } = string.Empty;
    }

    public class ReparticionApiDto
    {
        public int CdReparticion { get; set; }
        public string DsReparticion { get; set; } = string.Empty;
    }

    public class ResumenProduccionUsuarioApiDto
    {
        public int CantidadProcesadosHoy { get; set; }
        public int CantidadAsignados { get; set; }
        public int CantidadProcesados { get; set; }
        public int CantidadPendientes { get; set; }
    }

    public class AyudaControlApiDto
    {
        public string? DsCategoriaPlano { get; set; }
        public string? DsTipoPlano { get; set; }
        public string? DsExpediente { get; set; }
        public string? DsSeccion { get; set; }
        public string? DsManzana { get; set; }
        public string? DsParcela { get; set; }
        public string? DsDireccion { get; set; }
        public string? DsNumeroPlano { get; set; }
        public string? DsObservaciones { get; set; }
    }
}
