namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad para registrar el detalle de fallos al procesar la respuesta
    /// de OpenAI (error real de la API o excepcion al parsear el JSON).
    /// Persistida en TD_001_RESULTADO_IA_ERROR.
    /// </summary>
    public class ResultadoIAError
    {
        public int CdResultadoError { get; set; }
        public int CdLote { get; set; }
        public int CdArchivoPagina { get; set; }
        public string DsMotivoError { get; set; } = string.Empty;
        public string? DsRespuestaCruda { get; set; }
        public DateTime FeAlta { get; set; }
    }
}
