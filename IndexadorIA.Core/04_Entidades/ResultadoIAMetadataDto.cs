namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO de metadatos por pagina/resultado, proyectado desde la vista
    /// VW_001_RESULTADO_IA, usado para generar los CSV de finalizacion de lote.
    /// </summary>
    public class ResultadoIAMetadataDto
    {
        public string DsRutaCompleta { get; set; } = string.Empty;
        public string DsNombreArchivoOriginal { get; set; } = string.Empty;
        public string DsNombreArchivoFinal { get; set; } = string.Empty;
        public string DsCategoriaPlano { get; set; } = string.Empty;
        public string DsTipoPlano { get; set; } = string.Empty;
        public string DsAcronimo { get; set; } = string.Empty;
        public string DsDireccion { get; set; } = string.Empty;
        public string DsSeccion { get; set; } = string.Empty;
        public string DsManzana { get; set; } = string.Empty;
        public string DsParcela { get; set; } = string.Empty;
        public string DsExpediente { get; set; } = string.Empty;
        public string DsNumeroPlano { get; set; } = string.Empty;
        public int CdEstadoControl { get; set; }
        public string? DsObservaciones { get; set; }
    }
}
