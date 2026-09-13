namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO utilizado por la pantalla de Auditoría (FrmAuditarLote) para mostrar los
    /// registros de TD_001_RESULTADO_IA de un lote, junto con los datos de lote y
    /// control asociados necesarios para filtrar y validar.
    /// </summary>
    public class RegistroAuditoria
    {
        public int CdResultado { get; set; }

        // Datos de Lote (MOSTRAR y FILTRO)
        public int CdLote { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int CdEstadoLote { get; set; }
        public string? DsEstadoLote { get; set; }
        public DateTime? FeFinControl { get; set; }
        public int? CdUsuarioFinControl { get; set; }
        public string? DsUsuarioFinControl { get; set; }

        // Datos del registro (MOSTRAR)
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

        // Estado de control
        public int CdEstadoControl { get; set; }
        public string? DsEstadoControl { get; set; }
        public DateTime? FeControl { get; set; }
        public int? CdUsuarioControl { get; set; }
        public string? DsUsuarioControl { get; set; }
        public string? SnModificaDatos { get; set; }

        // Resultado de la validación del botón "Analizar" (no persistido)
        public bool EsInvalido { get; set; }

        // Indica si el registro ya fue revisado/editado por el auditor en esta sesión (no persistido)
        public bool EsRevisado { get; set; }
    }
}
