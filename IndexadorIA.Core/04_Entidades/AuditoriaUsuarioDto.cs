namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Fila del reporte "Auditoria x Usuario": resume, para un usuario auditor
    /// (TD_LOTE.cdUsuarioAuditor), la cantidad de lotes y la cantidad de planos
    /// (archivos) auditados, opcionalmente desagregado por fecha de auditoria
    /// (TD_LOTE.feAuditado).
    /// </summary>
    public class AuditoriaUsuarioDto
    {
        public int CdUsuario { get; set; }
        public string DsUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de auditoria del detalle diario. Es null cuando el reporte se
        /// muestra totalizado (sin el filtro "Detalle por Fecha").
        /// </summary>
        public DateTime? Fecha { get; set; }

        public int CantidadLotes { get; set; }
        public int CantidadPlanos { get; set; }
    }
}
