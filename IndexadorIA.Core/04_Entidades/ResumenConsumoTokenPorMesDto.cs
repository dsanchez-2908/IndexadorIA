namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO con el resumen de consumo de tokens de OpenAI agrupado por mes y modelo
    /// (para la grilla de detalle de la pantalla de reporte "Consumos IA")
    /// </summary>
    public class ResumenConsumoTokenPorMesDto
    {
        public string DsPeriodo { get; set; } = string.Empty;
        public string DsModelo { get; set; } = string.Empty;
        public long NuTotalTokensPrompt { get; set; }
        public long NuTotalTokensCompletion { get; set; }
        public long NuTotalTokensTotal { get; set; }
        public decimal NuCostoIngresoUsd { get; set; }
        public decimal NuCostoSalidaUsd { get; set; }
        public decimal NuCostoTotalUsd { get; set; }
        public int NuTotalArchivos { get; set; }
    }
}
