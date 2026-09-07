namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Texto de ayuda configurado por el administrador para cada uno de los 9 campos
    /// del panel de detalle de FrmVerLote. Existe un único registro global en
    /// TD_AYUDA_CONTROL.
    /// </summary>
    public class AyudaControl
    {
        public int CdAyudaControl { get; set; }
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
