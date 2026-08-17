namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Clase para representar la respuesta JSON que devuelve OpenAI
    /// </summary>
    public class RespuestaOpenAI
    {
        public string? archivo { get; set; }
        public string? categoriaPlano { get; set; }
        public string? tipoPlano { get; set; }
        public string? expediente { get; set; }
        public string? seccion { get; set; }
        public string? manzana { get; set; }
        public string? parcela { get; set; }
        public string? direccion { get; set; }
        public string? numeroPlano { get; set; }
        public ConfianzaRespuesta? confianza { get; set; }  // Singular, como lo devuelve OpenAI
    }

    /// <summary>
    /// Clase para representar los niveles de confianza de cada campo
    /// </summary>
    public class ConfianzaRespuesta
    {
        public decimal? categoriaPlano { get; set; }
        public decimal? tipoPlano { get; set; }
        public decimal? expediente { get; set; }
        public decimal? seccion { get; set; }
        public decimal? manzana { get; set; }
        public decimal? parcela { get; set; }
        public decimal? direccion { get; set; }
        public decimal? numeroPlano { get; set; }
    }
}
