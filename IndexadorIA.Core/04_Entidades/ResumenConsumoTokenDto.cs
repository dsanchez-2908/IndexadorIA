namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO con el resumen general de consumo de tokens de OpenAI (para la pantalla de reporte "Consumos IA")
    /// </summary>
    public class ResumenConsumoTokenDto
    {
        public long NuTotalTokensPrompt { get; set; }
        public long NuTotalTokensCompletion { get; set; }
        public long NuTotalTokensTotal { get; set; }
        public decimal NuCostoIngresoUsd { get; set; }
        public decimal NuCostoSalidaUsd { get; set; }
        public decimal NuCostoTotalUsd { get; set; }
        public int NuTotalArchivos { get; set; }
    }
}
