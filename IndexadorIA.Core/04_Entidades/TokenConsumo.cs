namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa el consumo de tokens de una llamada a la API de OpenAI
    /// </summary>
    public class TokenConsumo
    {
        public int CdToken { get; set; }
        public int CdResultado { get; set; }
        public int? NuTokensPrompt { get; set; }
        public int? NuTokensCompletion { get; set; }
        public int? NuTokensTotal { get; set; }
        public string? DsModelo { get; set; }
        public DateTime FeAlta { get; set; }
    }
}
