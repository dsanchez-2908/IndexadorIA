using IndexadorIA.Entidades;

namespace IndexadorIA.Api.Dtos
{
    /// <summary>
    /// Resumen de lote para la pantalla de Auditoria (FrmAuditar), espejo de
    /// Lote con los campos usados en la grilla de auditoria.
    /// </summary>
    public class LoteAuditoriaDto
    {
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int NuCantidadArchivos { get; set; }
        public DateTime? FeUltimoCambio { get; set; }
        public int NuEstadoControlado { get; set; }
        public int NuEstadoDatosIlegibles { get; set; }
        public int NuEstadoPaginaIlegible { get; set; }
        public string? DsUsuarioFinControl { get; set; }
        public string? DsEstado { get; set; }

        public static LoteAuditoriaDto DesdeEntidad(Lote lote) => new()
        {
            CdLote = lote.CdLote,
            DsNombreLote = lote.DsNombreLote,
            NuCantidadArchivos = lote.NuCantidadArchivos,
            FeUltimoCambio = lote.FeUltimoCambio,
            NuEstadoControlado = lote.NuEstadoControlado,
            NuEstadoDatosIlegibles = lote.NuEstadoDatosIlegibles,
            NuEstadoPaginaIlegible = lote.NuEstadoPaginaIlegible,
            DsUsuarioFinControl = lote.DsUsuarioFinControl,
            DsEstado = lote.DsEstado
        };
    }

    /// <summary>
    /// Registro de TD_001_RESULTADO_IA de un lote en auditoria, espejo de
    /// RegistroAuditoria, usado por FrmAuditarLote y FrmVerRegistroAuditoria.
    /// </summary>
    public class RegistroAuditoriaDto
    {
        public int CdResultado { get; set; }
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int CdEstadoLote { get; set; }
        public string? DsEstadoLote { get; set; }
        public DateTime? FeFinControl { get; set; }
        public int? CdUsuarioFinControl { get; set; }
        public string? DsUsuarioFinControl { get; set; }
        public int CdArchivoPagina { get; set; }
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
        public int CdEstadoControl { get; set; }
        public string? DsEstadoControl { get; set; }
        public DateTime? FeControl { get; set; }
        public int? CdUsuarioControl { get; set; }
        public string? DsUsuarioControl { get; set; }
        public string? SnModificaDatos { get; set; }

        public static RegistroAuditoriaDto DesdeEntidad(RegistroAuditoria r) => new()
        {
            CdResultado = r.CdResultado,
            CdLote = r.CdLote,
            DsNombreLote = r.DsNombreLote,
            CdEstadoLote = r.CdEstadoLote,
            DsEstadoLote = r.DsEstadoLote,
            FeFinControl = r.FeFinControl,
            CdUsuarioFinControl = r.CdUsuarioFinControl,
            DsUsuarioFinControl = r.DsUsuarioFinControl,
            CdArchivoPagina = r.CdArchivoPagina,
            CdCategoriaPlano = r.CdCategoriaPlano,
            DsCategoriaPlano = r.DsCategoriaPlano,
            CdTipoPlano = r.CdTipoPlano,
            DsTipoPlano = r.DsTipoPlano,
            DsExpediente = r.DsExpediente,
            DsSeccion = r.DsSeccion,
            DsManzana = r.DsManzana,
            DsParcela = r.DsParcela,
            DsDireccion = r.DsDireccion,
            DsNumeroPlano = r.DsNumeroPlano,
            DsObservaciones = r.DsObservaciones,
            CdEstadoControl = r.CdEstadoControl,
            DsEstadoControl = r.DsEstadoControl,
            FeControl = r.FeControl,
            CdUsuarioControl = r.CdUsuarioControl,
            DsUsuarioControl = r.DsUsuarioControl,
            SnModificaDatos = r.SnModificaDatos
        };
    }

    public class EstadoControlDto
    {
        public int CdEstado { get; set; }
        public string DsEstado { get; set; } = string.Empty;
    }

    /// <summary>
    /// Body para actualizar los datos de un resultado desde la pantalla de auditoria
    /// (FrmVerRegistroAuditoria). Idéntico a ActualizarResultadoRequestDto pero en
    /// ruta separada para poder distinguir el origen (auditoria) en el controller.
    /// </summary>
    public class ActualizarResultadoAuditoriaRequestDto
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

    public class CorreccionAuditoriaRequestDto
    {
        public string DsCampo { get; set; } = string.Empty;
        public string? DsValorAnterior { get; set; }
        public string? DsValorNuevo { get; set; }
    }

    public class RegistrarCorreccionesAuditoriaRequestDto
    {
        public List<CorreccionAuditoriaRequestDto> Correcciones { get; set; } = new();
    }
}
