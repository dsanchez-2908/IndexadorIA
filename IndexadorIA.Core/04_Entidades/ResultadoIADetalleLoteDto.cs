namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO de solo lectura para el detalle de registros de un lote, obtenido de
    /// VW_001_RESULTADO_IA, usado en la pantalla de "Ver Lote" previa a la asignación.
    /// </summary>
    public class ResultadoIADetalleLoteDto
    {
        public int CdLote { get; set; }
        public string? DsNombreLote { get; set; }
        public string? DsCategoriaPlano { get; set; }
        public string? DsTipoPlano { get; set; }
        public string? DsAcronimo { get; set; }
        public string? DsDireccion { get; set; }
        public string? DsSeccion { get; set; }
        public string? DsManzana { get; set; }
        public string? DsParcela { get; set; }
        public string? DsExpediente { get; set; }
        public string? DsNumeroPlano { get; set; }
    }
}
