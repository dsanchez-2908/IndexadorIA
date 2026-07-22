namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Enumeración para las esquinas de recorte
    /// </summary>
    public enum EsquinaRecorte
    {
        SuperiorIzquierda = 0,
        SuperiorDerecha = 1,
        InferiorIzquierda = 2,
        InferiorDerecha = 3
    }

    /// <summary>
    /// Configuración de recorte de imágenes
    /// </summary>
    public class ConfiguracionRecorte
    {
        /// <summary>
        /// Esquina desde donde se realiza el recorte
        /// </summary>
        public EsquinaRecorte EsquinaRecorte { get; set; } = EsquinaRecorte.InferiorDerecha;

        /// <summary>
        /// DPI para la conversión de PDF a imagen
        /// </summary>
        public int DPI { get; set; } = 300;

        /// <summary>
        /// Porcentaje vertical del recorte (0-100)
        /// </summary>
        public decimal PorcentajeVertical { get; set; } = 30;

        /// <summary>
        /// Porcentaje horizontal del recorte (0-100)
        /// </summary>
        public decimal PorcentajeHorizontal { get; set; } = 50;

        /// <summary>
        /// Indica si se debe ejecutar OCR
        /// </summary>
        public bool EjecutarOCR { get; set; } = false;

        /// <summary>
        /// Ruta del PDF temporal para preview
        /// </summary>
        public string? RutaPDFTemporal { get; set; }

        /// <summary>
        /// Calidad de compresión JPG (1-100)
        /// Reducido a 65 para optimizar tamaño en API de OpenAI
        /// </summary>
        public int CalidadJPG { get; set; } = 65;
    }
}
