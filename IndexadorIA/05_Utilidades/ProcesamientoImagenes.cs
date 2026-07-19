using IndexadorIA.Entidades;
using PdfiumViewer;
using System.Drawing;
using System.Drawing.Imaging;

namespace IndexadorIA.Utilidades
{
    /// <summary>
    /// Utilidades para procesamiento de imágenes PDF
    /// </summary>
    public static class ProcesamientoImagenes
    {
        /// <summary>
        /// Convierte un PDF a Bitmap con el DPI especificado
        /// </summary>
        public static Bitmap? ConvertirPDFaBitmap(string rutaPDF, int dpi)
        {
            try
            {
                if (!File.Exists(rutaPDF))
                {
                    throw new FileNotFoundException($"El archivo PDF no existe: {rutaPDF}");
                }

                using (var documento = PdfDocument.Load(rutaPDF))
                {
                    if (documento.PageCount == 0)
                    {
                        throw new InvalidOperationException("El PDF no contiene páginas");
                    }

                    // Renderizar la primera (y única) página del PDF
                    var image = documento.Render(0, dpi, dpi, PdfRenderFlags.CorrectFromDpi);

                    // Convertir Image a Bitmap si es necesario
                    if (image is Bitmap bitmap)
                    {
                        return bitmap;
                    }
                    else
                    {
                        // Crear un nuevo Bitmap a partir del Image
                        var bmp = new Bitmap(image);
                        image.Dispose();
                        return bmp;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al convertir PDF a Bitmap: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Recorta una imagen según la configuración especificada
        /// </summary>
        public static Bitmap RecortarImagen(Bitmap bitmapOriginal, EsquinaRecorte esquina, decimal porcVertical, decimal porcHorizontal)
        {
            try
            {
                // Validar porcentajes
                if (porcVertical < 0 || porcVertical > 100)
                    throw new ArgumentException("El porcentaje vertical debe estar entre 0 y 100");

                if (porcHorizontal < 0 || porcHorizontal > 100)
                    throw new ArgumentException("El porcentaje horizontal debe estar entre 0 y 100");

                int anchoOriginal = bitmapOriginal.Width;
                int altoOriginal = bitmapOriginal.Height;

                // Calcular dimensiones del recorte
                int anchoRecorte = (int)(anchoOriginal * porcHorizontal / 100);
                int altoRecorte = (int)(altoOriginal * porcVertical / 100);

                // Calcular posición del recorte según la esquina
                int x = 0, y = 0;

                switch (esquina)
                {
                    case EsquinaRecorte.SuperiorIzquierda:
                        x = 0;
                        y = 0;
                        break;

                    case EsquinaRecorte.SuperiorDerecha:
                        x = anchoOriginal - anchoRecorte;
                        y = 0;
                        break;

                    case EsquinaRecorte.InferiorIzquierda:
                        x = 0;
                        y = altoOriginal - altoRecorte;
                        break;

                    case EsquinaRecorte.InferiorDerecha:
                        x = anchoOriginal - anchoRecorte;
                        y = altoOriginal - altoRecorte;
                        break;
                }

                // Crear rectángulo de recorte
                var rectangulo = new Rectangle(x, y, anchoRecorte, altoRecorte);

                // Recortar la imagen
                var bitmapRecortado = bitmapOriginal.Clone(rectangulo, bitmapOriginal.PixelFormat);

                return bitmapRecortado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al recortar imagen: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Guarda un Bitmap como JPG con la calidad especificada
        /// </summary>
        public static void GuardarJPG(Bitmap bitmap, string rutaDestino, int calidad = 90)
        {
            try
            {
                // Validar calidad
                if (calidad < 1 || calidad > 100)
                    throw new ArgumentException("La calidad debe estar entre 1 y 100");

                // Obtener el codec JPG
                var jpegCodec = ImageCodecInfo.GetImageEncoders()
                    .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                if (jpegCodec == null)
                    throw new Exception("No se encontró el codec JPEG");

                // Configurar parámetros de calidad
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, calidad);

                // Crear directorio si no existe
                var directorio = Path.GetDirectoryName(rutaDestino);
                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                // Guardar la imagen
                bitmap.Save(rutaDestino, jpegCodec, encoderParameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar JPG: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Convierte una imagen JPG a string Base64
        /// </summary>
        public static string ConvertirImagenABase64(string rutaJPG)
        {
            try
            {
                if (!File.Exists(rutaJPG))
                    throw new FileNotFoundException($"El archivo JPG no existe: {rutaJPG}");

                byte[] imageBytes = File.ReadAllBytes(rutaJPG);
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al convertir imagen a Base64: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Guarda un string Base64 en un archivo
        /// </summary>
        public static void GuardarBase64EnArchivo(string base64String, string rutaDestino)
        {
            try
            {
                // Crear directorio si no existe
                var directorio = Path.GetDirectoryName(rutaDestino);
                if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                File.WriteAllText(rutaDestino, base64String);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar archivo Base64: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Calcula el rectángulo de recorte según la configuración
        /// </summary>
        public static Rectangle CalcularRectanguloRecorte(int anchoTotal, int altoTotal, EsquinaRecorte esquina, decimal porcVertical, decimal porcHorizontal)
        {
            // Calcular dimensiones del recorte
            int anchoRecorte = (int)(anchoTotal * porcHorizontal / 100);
            int altoRecorte = (int)(altoTotal * porcVertical / 100);

            // Calcular posición según la esquina
            int x = 0, y = 0;

            switch (esquina)
            {
                case EsquinaRecorte.SuperiorIzquierda:
                    x = 0;
                    y = 0;
                    break;

                case EsquinaRecorte.SuperiorDerecha:
                    x = anchoTotal - anchoRecorte;
                    y = 0;
                    break;

                case EsquinaRecorte.InferiorIzquierda:
                    x = 0;
                    y = altoTotal - altoRecorte;
                    break;

                case EsquinaRecorte.InferiorDerecha:
                    x = anchoTotal - anchoRecorte;
                    y = altoTotal - altoRecorte;
                    break;
            }

            return new Rectangle(x, y, anchoRecorte, altoRecorte);
        }
    }
}
