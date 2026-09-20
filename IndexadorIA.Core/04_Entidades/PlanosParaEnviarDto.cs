namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO con los indicadores del reporte "Planos para enviar": resume los lotes
    /// pendientes de finalizar, los lotes finalizados listos para enviar y el
    /// total de casos especiales de toda la base.
    /// </summary>
    public class PlanosParaEnviarDto
    {
        // Pendiente de Finalizar (TD_LOTE.cdEstadoLote = 9)
        public int TotalLotesPendienteFinalizar { get; set; }
        public int TotalPlanosCorrectosPendienteFinalizar { get; set; }
        public int TotalPlanosDatosIlegiblesPendienteFinalizar { get; set; }
        public int TotalPlanosIlegiblesPendienteFinalizar { get; set; }
        public int TotalPlanosCasosEspecialesPendienteFinalizar { get; set; }

        // Lotes Finalizados (Para Enviar) (TD_LOTE.cdEstadoLote = 10)
        public int TotalLotesFinalizadosParaEnviar { get; set; }
        public int TotalPlanosCorrectosFinalizados { get; set; }
        public int TotalPlanosDatosIlegiblesFinalizados { get; set; }
        public int TotalPlanosIlegiblesFinalizados { get; set; }

        // Total de Planos Casos Especiales en base
        public int TotalCasosEspecialesEnBase { get; set; }
    }
}
