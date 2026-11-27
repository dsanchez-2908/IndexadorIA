namespace IndexadorIA.Entidades
{
    /// <summary>Archivo original dentro de un lote final (grilla de originales del Control Final).</summary>
    public class ArchivoLoteFinalDto
    {
        public int CdArchivo { get; set; }
        public string DsNombreArchivo { get; set; } = string.Empty;
        public string DsRutaCompleta { get; set; } = string.Empty;
        public int CdEstado { get; set; }
        public string DsEstado => CdEstado switch { 2 => "Finalizado", 3 => "Devuelto para SMP", _ => "Pendiente" };
        public int NuPaginas { get; set; }
        public int NuFinalizadas { get; set; }
        public int NuPendientes => NuPaginas - NuFinalizadas;
    }

    /// <summary>Resumen por usuario para el Monitor de Lotes Finales.</summary>
    public class MonitorLoteFinalUsuarioDto
    {
        public int CdUsuario { get; set; }
        public string DsUsuario { get; set; } = string.Empty;
        public int NuLotes { get; set; }
        public int NuLotesAsignados { get; set; }
        public int NuArchivos { get; set; }
        public int NuPendientes { get; set; }
        public int NuFinalizados { get; set; }
        public int NuDevueltosSmp { get; set; }
        public int NuPaginas { get; set; }
        public int NuPaginasFinalizadas { get; set; }
        public int NuPaginasPendientes => NuPaginas - NuPaginasFinalizadas;
    }

    /// <summary>Estado de control (TD_ESTADOS, proceso CONTROL) para el combo del Control Final.</summary>
    public class EstadoControlItem
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    /// <summary>Pagina de un archivo original con los datos de TD_001_RESULTADO_IA (editable en el Control Final).</summary>
    public class PaginaFinalDto
    {
        public bool Seleccionado { get; set; }
        public bool Modificado { get; set; }
        public int CdResultado { get; set; }
        public int CdArchivoPagina { get; set; }
        public int NuPagina { get; set; }
        public string DsRutaPagina { get; set; } = string.Empty;
        public int CdEstadoControl { get; set; }
        public string DsEstadoControl { get; set; } = string.Empty;
        public int? CdCategoriaPlano { get; set; }
        public int? CdTipoPlano { get; set; }
        public string DsCategoria { get; set; } = string.Empty;
        public string DsTipo { get; set; } = string.Empty;
        public string DsAcronimo { get; set; } = string.Empty;
        public string DsExpediente { get; set; } = string.Empty;
        public string DsSeccion { get; set; } = string.Empty;
        public string DsManzana { get; set; } = string.Empty;
        public string DsParcela { get; set; } = string.Empty;
        public string DsDireccion { get; set; } = string.Empty;
        public string DsNumeroPlano { get; set; } = string.Empty;
        public string DsObservaciones { get; set; } = string.Empty;

        public bool EsIlegible => CdEstadoControl == 3 || CdEstadoControl == 4;
    }
}
