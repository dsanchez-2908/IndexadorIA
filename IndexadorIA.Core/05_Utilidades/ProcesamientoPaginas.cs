using IndexadorIA.Entidades;
using PdfiumViewer;
using System.Drawing;
using System.Drawing.Imaging;

namespace IndexadorIA.Utilidades
{
    /// <summary>
    /// Modo de giro a aplicar sobre las páginas al separarlas
    /// </summary>
    public enum ModoGiro
    {
        Ninguno,
        Automatico,
        Derecha90,
        Izquierda90
    }

    public class ProcesamientoPaginas
    {
        /// <summary>
        /// Carpeta temporal donde se guardan las imágenes PNG de las páginas giradas
        /// automáticamente con baja confianza, para que el usuario las revise en la
        /// pantalla "Revisión de rotaciones" al finalizar el procesamiento del lote.
        /// </summary>
        public static string CarpetaRevisionGiro => Path.Combine(Path.GetTempPath(), "IndexadorIA_RevisionGiro");

        /// <summary>
        /// Elimina la carpeta temporal con las imágenes de revisión de rotaciones,
        /// junto con todo su contenido. Se llama al finalizar la pantalla de revisión.
        /// </summary>
        public static void LimpiarCarpetaRevisionGiro()
        {
            try
            {
                if (Directory.Exists(CarpetaRevisionGiro))
                {
                    Directory.Delete(CarpetaRevisionGiro, recursive: true);
                }
            }
            catch (Exception ex)
            {
                Datos.LogDAL.RegistrarLog(
                    Entidades.LogRegistro.Niveles.WARNING,
                    "ProcesamientoPaginas.LimpiarCarpetaRevisionGiro",
                    "No se pudo eliminar la carpeta temporal de revisión de rotaciones.",
                    ex);
            }
        }

        /// <summary>
        /// Guarda una copia PNG (ya rotada, tal como quedó aplicada) de una página con
        /// giro automático de baja confianza, para mostrarla luego en la pantalla de
        /// revisión manual. Se reduce la resolución respecto del render original (300
        /// DPI) porque solo se usa como miniatura de previsualización; esto evita
        /// manipular bitmaps enormes en la pantalla de revisión, que era la causa de
        /// la lentitud al rotar/mostrar imágenes.
        /// </summary>
        private const int AnchoMaximoImagenRevision = 500;

        private static string? GuardarImagenTemporalRevision(Image imagen, int gradosYaAplicados, string nombreArchivoPagina)
        {
            try
            {
                Directory.CreateDirectory(CarpetaRevisionGiro);

                string nombreImagen = $"{Path.GetFileNameWithoutExtension(nombreArchivoPagina)}_{Guid.NewGuid():N}.png";
                string rutaImagen = Path.Combine(CarpetaRevisionGiro, nombreImagen);

                using (var bitmap = new Bitmap(imagen))
                using (var bitmapRotado = gradosYaAplicados > 0 ? RotarImagen(bitmap, gradosYaAplicados) : bitmap.Clone() as Image)
                using (var bitmapReducido = ReducirResolucion(bitmapRotado, AnchoMaximoImagenRevision))
                {
                    bitmapReducido.Save(rutaImagen, ImageFormat.Png);
                }

                return rutaImagen;
            }
            catch (Exception ex)
            {
                Datos.LogDAL.RegistrarLog(
                    Entidades.LogRegistro.Niveles.WARNING,
                    "ProcesamientoPaginas.GuardarImagenTemporalRevision",
                    $"No se pudo guardar la imagen temporal de revisión para {nombreArchivoPagina}",
                    ex);
                return null;
            }
        }

        /// <summary>
        /// Reduce una imagen a un ancho máximo (manteniendo la proporción), usada para
        /// generar miniaturas livianas en la pantalla de revisión de rotaciones.
        /// </summary>
        private static Bitmap ReducirResolucion(Image imagen, int anchoMaximo)
        {
            if (imagen.Width <= anchoMaximo)
            {
                return new Bitmap(imagen);
            }

            double escala = (double)anchoMaximo / imagen.Width;
            int nuevoAncho = anchoMaximo;
            int nuevoAlto = (int)Math.Round(imagen.Height * escala);

            var bitmapReducido = new Bitmap(nuevoAncho, nuevoAlto);
            using (var graphics = Graphics.FromImage(bitmapReducido))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                graphics.DrawImage(imagen, 0, 0, nuevoAncho, nuevoAlto);
            }

            return bitmapReducido;
        }

        /// <summary>
        /// Intenta eliminar un archivo con reintentos (para manejar bloqueos temporales)
        /// </summary>
        private static bool IntentarEliminarArchivo(string rutaArchivo, int maxIntentos = 3, int esperaMilisegundos = 100)
        {
            for (int intento = 0; intento < maxIntentos; intento++)
            {
                try
                {
                    if (File.Exists(rutaArchivo))
                    {
                        // Forzar garbage collection para liberar handles
                        if (intento > 0)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            Thread.Sleep(esperaMilisegundos);
                        }

                        File.Delete(rutaArchivo);
                        return true;
                    }
                    return true; // No existe, consideramos éxito
                }
                catch (IOException)
                {
                    if (intento == maxIntentos - 1)
                        return false; // Último intento falló

                    Thread.Sleep(esperaMilisegundos);
                }
                catch
                {
                    return false; // Otro tipo de error
                }
            }
            return false;
        }

        /// <summary>
        /// Resultado del procesamiento de una página
        /// </summary>
        public class ResultadoPagina
        {
            public int NumeroPagina { get; set; }
            public string NombreArchivo { get; set; } = string.Empty;
            public string RutaCompleta { get; set; } = string.Empty;
            public bool SeGiro { get; set; }
            public int GradosRotacion { get; set; }
            public bool EsPosibleBlanca { get; set; }
            public bool Exito { get; set; }
            public string? Mensaje { get; set; }

            /// <summary>
            /// Indica que el giro automático se aplicó igual, pero la detección de
            /// orientación (Tesseract OSD) tuvo baja confianza, por lo que conviene
            /// que el usuario revise manualmente esta página.
            /// </summary>
            public bool BajaConfianzaGiro { get; set; }

            /// <summary>
            /// Ruta de la imagen temporal (PNG) generada para poder mostrar esta
            /// página en la pantalla de revisión de rotaciones de baja confianza.
            /// Solo se completa cuando BajaConfianzaGiro es true.
            /// </summary>
            public string? RutaImagenTemporalRevision { get; set; }
        }

        /// <summary>
        /// Separa un archivo PDF en páginas individuales usando extracción nativa
        /// Maneja archivos corruptos de forma segura
        /// </summary>
        public static List<ResultadoPagina> SepararPaginasPDF(
            string rutaArchivo,
            string carpetaDestino,
            int secuenciaInicial,
            ModoGiro modoGiro,
            bool detectarBlancas)
        {
            var resultados = new List<ResultadoPagina>();

            PdfDocument? documentoPdfium = null;
            try
            {
                // Intentar cargar el PDF con manejo de errores específico
                try
                {
                    documentoPdfium = PdfDocument.Load(rutaArchivo);
                }
                catch (Exception exLoad)
                {
                    // PDF corrupto o inválido
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.ERROR,
                        "ProcesamientoPaginas.SepararPaginasPDF",
                        $"No se pudo cargar el PDF: {rutaArchivo}",
                        exLoad);

                    // No lanzar excepción - devolver lista vacía con log
                    return resultados;
                }

                int totalPaginas = documentoPdfium.PageCount;

                // Validar que el PDF tenga al menos una página
                if (totalPaginas == 0)
                {
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.WARNING,
                        "ProcesamientoPaginas.SepararPaginasPDF",
                        $"PDF sin páginas: {rutaArchivo}");

                    // No lanzar excepción - devolver lista vacía
                    return resultados;
                }

                for (int i = 0; i < totalPaginas; i++)
                {
                    var resultado = new ResultadoPagina
                    {
                        NumeroPagina = i + 1,
                        Exito = false
                    };

                    try
                    {
                        // Generar nombre de archivo con secuencia
                        int numeroSecuencia = secuenciaInicial + i;
                        string nombreArchivo = $"{numeroSecuencia:D8}.pdf";
                        string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

                        resultado.NombreArchivo = nombreArchivo;
                        resultado.RutaCompleta = rutaCompleta;

                        // Renderizar página a imagen SOLO para análisis (detección de blanca y rotación)
                        using (var imagen = documentoPdfium.Render(i, 300, 300, PdfRenderFlags.CorrectFromDpi))
                        {
                            // Detectar si es página blanca
                            if (detectarBlancas)
                            {
                                resultado.EsPosibleBlanca = EsPaginaBlanca(imagen);
                            }

                            // Detectar rotación si es necesario
                            int rotacion = 0;
                            bool bajaConfianzaGiro = false;
                            if (modoGiro != ModoGiro.Ninguno)
                            {
                                rotacion = DeterminarRotacion(imagen, modoGiro, out bajaConfianzaGiro);
                                if (rotacion > 0)
                                {
                                    resultado.SeGiro = true;
                                    resultado.GradosRotacion = rotacion;
                                }

                                if (bajaConfianzaGiro)
                                {
                                    resultado.BajaConfianzaGiro = true;
                                    resultado.RutaImagenTemporalRevision = GuardarImagenTemporalRevision(imagen, rotacion, nombreArchivo);
                                }
                            }

                            // Intentar extraer usando el método de copia y eliminación (más robusto)
                            bool extraidoConExito = false;
                            try
                            {
                                // iText7 usa índice base 1, por eso sumamos 1
                                if (rotacion > 0)
                                {
                                    ExtraerYRotarPaginaPorCopiaYEliminacion(rutaArchivo, i + 1, rutaCompleta, rotacion);
                                }
                                else
                                {
                                    ExtraerPaginaPorCopiaYEliminacion(rutaArchivo, i + 1, rutaCompleta);
                                }
                                extraidoConExito = true;
                            }
                            catch (Exception exCopiaYEliminacion)
                            {
                                // Si falla el método de copia, intentar método de copia de página
                                Datos.LogDAL.RegistrarLog(
                                    Entidades.LogRegistro.Niveles.WARNING,
                                    "ProcesamientoPaginas.SepararPaginasPDF",
                                    $"Método de copia y eliminación falló para página {i + 1} de {Path.GetFileName(rutaArchivo)}, intentando copia directa de página. Error: {exCopiaYEliminacion.Message}",
                                    exCopiaYEliminacion);

                                try
                                {
                                    // Eliminar archivo parcial si existe (con reintentos)
                                    if (!IntentarEliminarArchivo(rutaCompleta))
                                    {
                                        Datos.LogDAL.RegistrarLog(
                                            Entidades.LogRegistro.Niveles.WARNING,
                                            "ProcesamientoPaginas.SepararPaginasPDF",
                                            $"No se pudo eliminar archivo parcial bloqueado: {Path.GetFileName(rutaCompleta)}");
                                    }

                                    // Intentar método de copia directa de página
                                    if (rotacion > 0)
                                    {
                                        ExtraerYRotarPaginaPDF(rutaArchivo, i + 1, rutaCompleta, rotacion);
                                    }
                                    else
                                    {
                                        ExtraerPaginaPDFNativa(rutaArchivo, i + 1, rutaCompleta);
                                    }
                                    extraidoConExito = true;

                                    Datos.LogDAL.RegistrarLog(
                                        Entidades.LogRegistro.Niveles.INFO,
                                        "ProcesamientoPaginas.SepararPaginasPDF",
                                        $"Página {i + 1} extraída exitosamente usando método de copia directa (fallback)");
                                }
                                catch (Exception exCopiaDirecta)
                                {
                                    // Si también falla la copia directa, usar método de imagen (último recurso)
                                    Datos.LogDAL.RegistrarLog(
                                        Entidades.LogRegistro.Niveles.WARNING,
                                        "ProcesamientoPaginas.SepararPaginasPDF",
                                        $"Método de copia directa falló para página {i + 1}, usando renderizado a imagen como último recurso. Error: {exCopiaDirecta.Message}",
                                        exCopiaDirecta);

                                    try
                                    {
                                        // Eliminar archivo parcial si existe (con reintentos)
                                        if (!IntentarEliminarArchivo(rutaCompleta))
                                        {
                                            Datos.LogDAL.RegistrarLog(
                                                Entidades.LogRegistro.Niveles.WARNING,
                                                "ProcesamientoPaginas.SepararPaginasPDF",
                                                $"No se pudo eliminar archivo parcial bloqueado antes de renderizar imagen: {Path.GetFileName(rutaCompleta)}");
                                        }

                                        // Usar método de renderizado a imagen
                                        ExtraerPaginaPorImagen(documentoPdfium, i, rutaCompleta, rotacion);
                                        extraidoConExito = true;

                                        // Registrar éxito del fallback
                                        Datos.LogDAL.RegistrarLog(
                                            Entidades.LogRegistro.Niveles.INFO,
                                            "ProcesamientoPaginas.SepararPaginasPDF",
                                            $"Página {i + 1} extraída exitosamente usando método de imagen (último recurso)");
                                    }
                                    catch (Exception exImagen)
                                    {
                                        // Eliminar archivo parcial de 0 KB si existe (con reintentos)
                                        IntentarEliminarArchivo(rutaCompleta);

                                        // Registrar que todos los métodos fallaron
                                        Datos.LogDAL.RegistrarLog(
                                            Entidades.LogRegistro.Niveles.ERROR,
                                            "ProcesamientoPaginas.SepararPaginasPDF",
                                            $"Todos los métodos de extracción fallaron para página {i + 1}. Copia/Eliminación: {exCopiaYEliminacion.Message}, Copia Directa: {exCopiaDirecta.Message}, Imagen: {exImagen.Message}",
                                            exImagen);

                                        // Lanzar excepción compuesta
                                        throw new Exception($"Todos los métodos fallaron: CopiaElim=[{exCopiaYEliminacion.Message}] | CopiaDirecta=[{exCopiaDirecta.Message}] | Imagen=[{exImagen.Message}]");
                                    }
                                }
                            }
                        }

                        resultado.Exito = true;
                    }
                    catch (Exception ex)
                    {
                        resultado.Exito = false;
                        resultado.Mensaje = $"Error al procesar página {i + 1}: {ex.Message}";

                        // Registrar error en log
                        Datos.LogDAL.RegistrarLog(
                            Entidades.LogRegistro.Niveles.ERROR,
                            "ProcesamientoPaginas.SepararPaginasPDF",
                            $"Error en página {i + 1} de {rutaArchivo}",
                            ex);
                    }

                    resultados.Add(resultado);
                }
            }
            catch (Exception ex)
            {
                // Registrar error crítico
                Datos.LogDAL.RegistrarLog(
                    Entidades.LogRegistro.Niveles.CRITICAL,
                    "ProcesamientoPaginas.SepararPaginasPDF",
                    $"Error crítico al separar páginas de {rutaArchivo}",
                    ex);

                // No relanzar - ya está registrado
            }
            finally
            {
                // Liberar recursos del documento PDF
                documentoPdfium?.Dispose();
            }

            return resultados;
        }

        /// <summary>
        /// Procesa un archivo JPG (simplemente lo copia con el nuevo nombre)
        /// </summary>
        public static ResultadoPagina ProcesarArchivoJPG(
            string rutaArchivo,
            string carpetaDestino,
            int secuencia,
            ModoGiro modoGiro,
            bool detectarBlancas)
        {
            var resultado = new ResultadoPagina
            {
                NumeroPagina = 1,
                Exito = false
            };

            try
            {
                // Generar nombre de archivo con secuencia
                string nombreArchivo = $"{secuencia:D8}.jpg";
                string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

                resultado.NombreArchivo = nombreArchivo;
                resultado.RutaCompleta = rutaCompleta;

                // Cargar imagen para análisis
                using (var imagen = Image.FromFile(rutaArchivo))
                {
                    // Detectar si es imagen blanca
                    if (detectarBlancas)
                    {
                        resultado.EsPosibleBlanca = EsPaginaBlanca(imagen);
                    }

                    // Detectar y aplicar rotación si es necesario
                    if (modoGiro != ModoGiro.Ninguno)
                    {
                        var rotacion = DeterminarRotacion(imagen, modoGiro, out bool bajaConfianzaGiro);
                        if (rotacion > 0)
                        {
                            resultado.SeGiro = true;
                            resultado.GradosRotacion = rotacion;

                            // Rotar y guardar
                            using (var imagenRotada = RotarImagen(imagen, rotacion))
                            {
                                imagenRotada.Save(rutaCompleta, ImageFormat.Jpeg);
                            }
                        }
                        else
                        {
                            // Copiar sin rotación
                            File.Copy(rutaArchivo, rutaCompleta, true);
                        }

                        if (bajaConfianzaGiro)
                        {
                            resultado.BajaConfianzaGiro = true;
                            resultado.RutaImagenTemporalRevision = GuardarImagenTemporalRevision(imagen, rotacion, resultado.NombreArchivo);
                        }
                    }
                    else
                    {
                        // Copiar sin rotación
                        File.Copy(rutaArchivo, rutaCompleta, true);
                    }
                }

                resultado.Exito = true;
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.Mensaje = $"Error al procesar archivo JPG: {ex.Message}";
            }

            return resultado;
        }

        /// <summary>
        /// Detecta si una imagen está mayormente en blanco
        /// </summary>
        private static bool EsPaginaBlanca(Image imagen)
        {
            try
            {
                using (var bitmap = new Bitmap(imagen))
                {
                    // Reducir tamaño para análisis rápido
                    int anchoMuestra = Math.Min(200, bitmap.Width);
                    int altoMuestra = Math.Min(200, bitmap.Height);
                    int totalPixeles = 0;
                    int pixelesNoBlanccos = 0;

                    int saltoX = bitmap.Width / anchoMuestra;
                    int saltoY = bitmap.Height / altoMuestra;

                    for (int y = 0; y < bitmap.Height; y += saltoY)
                    {
                        for (int x = 0; x < bitmap.Width; x += saltoX)
                        {
                            var pixel = bitmap.GetPixel(x, y);
                            totalPixeles++;

                            // Considerar pixel no blanco si su brillo es menor a 240
                            int brillo = (pixel.R + pixel.G + pixel.B) / 3;
                            if (brillo < 240)
                            {
                                pixelesNoBlanccos++;
                            }
                        }
                    }

                    // Si menos del 5% de los píxeles no son blancos, considerar página blanca
                    double porcentajeNoBlanco = (double)pixelesNoBlanccos / totalPixeles * 100;
                    return porcentajeNoBlanco < 5.0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determina la rotación a aplicar en función del modo de giro elegido por el usuario.
        /// - Manual (Derecha90/Izquierda90): se aplica el giro fijo indicado, sin analizar la imagen.
        /// - Automático: se usa Tesseract OSD (Orientation and Script Detection) para detectar
        ///   la orientación real del texto de la página, mucho más preciso que heurísticas de
        ///   densidad de píxeles en los bordes.
        /// </summary>
        private static int DeterminarRotacion(Image imagen, ModoGiro modoGiro, out bool bajaConfianza)
        {
            bajaConfianza = false;
            switch (modoGiro)
            {
                case ModoGiro.Derecha90:
                    return 90;
                case ModoGiro.Izquierda90:
                    return 270;
                case ModoGiro.Automatico:
                    return DetectarRotacionConOSD(imagen, out bajaConfianza);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Detecta la rotación necesaria usando el ejecutable de Tesseract instalado en el
        /// sistema (tesseract.exe, disponible en el PATH) en modo OSD (Orientation and Script
        /// Detection). Se invoca como proceso externo en lugar de usar la librería NuGet
        /// Tesseract, ya que el binario oficial del sistema (con el modelo de idioma spa
        /// instalado) da resultados mucho más precisos y consistentes con el flujo que ya
        /// se usaba manualmente con Python/ocrmypdf.
        /// </summary>
        private static int DetectarRotacionConOSD(Image imagen, out bool bajaConfianza)
        {
            bajaConfianza = false;
            string? rutaTemporalImagen = null;
            string? rutaTemporalSalida = null;
            try
            {
                string rutaTesseract = ObtenerRutaEjecutableTesseract();
                if (string.IsNullOrEmpty(rutaTesseract))
                {
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.ERROR,
                        "ProcesamientoPaginas.DetectarRotacionConOSD",
                        "No se encontró tesseract.exe instalado en el sistema (PATH ni ubicaciones típicas de instalación). " +
                        "No se puede aplicar el giro automático de las páginas. Instale Tesseract OCR o seleccione otro modo de giro.");
                    return 0;
                }

                rutaTemporalImagen = Path.Combine(Path.GetTempPath(), $"osd_{Guid.NewGuid():N}.png");
                rutaTemporalSalida = Path.Combine(Path.GetTempPath(), $"osd_{Guid.NewGuid():N}");

                using (var bitmap = new Bitmap(imagen))
                {
                    bitmap.Save(rutaTemporalImagen, ImageFormat.Png);
                }

                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = rutaTesseract,
                    Arguments = $"\"{rutaTemporalImagen}\" \"{rutaTemporalSalida}\" --psm 0",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                string salidaEstandar;
                string salidaError;
                using (var proceso = System.Diagnostics.Process.Start(psi))
                {
                    if (proceso == null)
                    {
                        return 0;
                    }

                    salidaEstandar = proceso.StandardOutput.ReadToEnd();
                    salidaError = proceso.StandardError.ReadToEnd();
                    proceso.WaitForExit(15000);
                }

                string rutaArchivoOsd = rutaTemporalSalida + ".osd";
                string textoOsd = File.Exists(rutaArchivoOsd)
                    ? File.ReadAllText(rutaArchivoOsd)
                    : salidaEstandar;

                if (string.IsNullOrWhiteSpace(textoOsd))
                {
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.WARNING,
                        "ProcesamientoPaginas.DetectarRotacionConOSD",
                        $"tesseract --psm 0 no devolvió resultados de OSD. Salida error: {salidaError}");
                    return 0;
                }

                // La salida de "tesseract --psm 0" contiene líneas del tipo:
                //   Rotate: 90
                //   Orientation confidence: 6.62
                // "Rotate" indica cuántos grados hay que rotar la imagen en sentido
                // horario para corregir su orientación, pero esa detección puede ser
                // poco confiable en páginas con poco texto (planos con sellos,
                // membretes, etc.), donde Tesseract llega a confundir 0°/180° (o
                // 90°/270°) por la simetría de algunos caracteres. Por eso se exige
                // un mínimo de confianza antes de aplicar el giro; de lo contrario
                // se omite y se deja registro para poder diagnosticarlo.
                const double confianzaMinima = 3.0;

                int? gradosDetectados = null;
                double? confianzaOrientacion = null;

                foreach (var linea in textoOsd.Split('\n'))
                {
                    if (linea.StartsWith("Rotate:", StringComparison.OrdinalIgnoreCase))
                    {
                        string valor = linea.Split(':')[1].Trim();
                        if (int.TryParse(valor, out int gradosRotar))
                        {
                            gradosDetectados = ((gradosRotar % 360) + 360) % 360;
                        }
                    }
                    else if (linea.StartsWith("Orientation confidence:", StringComparison.OrdinalIgnoreCase))
                    {
                        string valor = linea.Split(':')[1].Trim();
                        if (double.TryParse(valor, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double confianza))
                        {
                            confianzaOrientacion = confianza;
                        }
                    }
                }

                if (gradosDetectados == null)
                {
                    return 0;
                }

                // Ya NO se omite el giro por baja confianza: se aplica igual, pero
                // se marca la página para que el usuario la revise manualmente en la
                // pantalla de "Revisión de rotaciones" al finalizar el lote.
                if (confianzaOrientacion.HasValue && confianzaOrientacion.Value < confianzaMinima)
                {
                    bajaConfianza = true;
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.WARNING,
                        "ProcesamientoPaginas.DetectarRotacionConOSD",
                        $"Detección de rotación automática con baja confianza ({confianzaOrientacion.Value:0.00}). Se aplicó igual el giro de {gradosDetectados.Value}°, pero se marcó la página para revisión manual.");
                }

                return gradosDetectados.Value;
            }
            catch (Exception ex)
            {
                Datos.LogDAL.RegistrarLog(
                    Entidades.LogRegistro.Niveles.WARNING,
                    "ProcesamientoPaginas.DetectarRotacionConOSD",
                    "Error al detectar rotación automática con tesseract.exe, se omite el giro.",
                    ex);
                return 0;
            }
            finally
            {
                if (rutaTemporalImagen != null)
                {
                    IntentarEliminarArchivo(rutaTemporalImagen);
                }
                if (rutaTemporalSalida != null)
                {
                    IntentarEliminarArchivo(rutaTemporalSalida + ".osd");
                }
            }
        }

        private static string? _rutaTesseractCacheada;
        private static bool _rutaTesseractBuscada;

        /// <summary>
        /// Indica si tesseract.exe está instalado y disponible en el sistema (PATH o
        /// ubicaciones típicas de instalación). Debe usarse antes de procesar un lote con
        /// giro automático, para poder avisar al usuario ANTES de procesar en lugar de
        /// omitir el giro silenciosamente.
        /// </summary>
        public static bool EstaTesseractInstalado()
        {
            return !string.IsNullOrEmpty(ObtenerRutaEjecutableTesseract());
        }

        /// <summary>
        /// Localiza el ejecutable tesseract.exe instalado en el sistema (PATH o ubicaciones
        /// típicas de instalación), el mismo binario usado manualmente desde consola/Python.
        /// </summary>
        private static string? ObtenerRutaEjecutableTesseract()
        {
            if (_rutaTesseractBuscada)
            {
                return _rutaTesseractCacheada;
            }

            _rutaTesseractBuscada = true;

            // 1) Buscar en el PATH del sistema
            string? rutaPath = Environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrEmpty(rutaPath))
            {
                foreach (var carpeta in rutaPath.Split(Path.PathSeparator))
                {
                    try
                    {
                        string candidato = Path.Combine(carpeta, "tesseract.exe");
                        if (File.Exists(candidato))
                        {
                            _rutaTesseractCacheada = candidato;
                            return _rutaTesseractCacheada;
                        }
                    }
                    catch
                    {
                        // Ignorar entradas de PATH inválidas
                    }
                }
            }

            // 2) Ubicaciones típicas de instalación en Windows
            string[] rutasComunes =
            {
                @"C:\Program Files\Tesseract-OCR\tesseract.exe",
                @"C:\Program Files (x86)\Tesseract-OCR\tesseract.exe"
            };

            foreach (var ruta in rutasComunes)
            {
                if (File.Exists(ruta))
                {
                    _rutaTesseractCacheada = ruta;
                    return _rutaTesseractCacheada;
                }
            }

            _rutaTesseractCacheada = null;
            return null;
        }

        /// <summary>
        /// Rota una imagen en los grados especificados
        /// </summary>
        private static Image RotarImagen(Image imagen, int grados)
        {
            var rotateFlip = grados switch
            {
                90 => RotateFlipType.Rotate90FlipNone,
                180 => RotateFlipType.Rotate180FlipNone,
                270 => RotateFlipType.Rotate270FlipNone,
                _ => RotateFlipType.RotateNoneFlipNone
            };

            var imagenRotada = new Bitmap(imagen);
            imagenRotada.RotateFlip(rotateFlip);
            return imagenRotada;
        }

        /// <summary>
        /// Extrae una página específica de un PDF sin modificaciones usando iText7
        /// Mantiene el PDF nativo sin conversión a imagen
        /// </summary>
        private static void ExtraerPaginaPDF(PdfDocument documentoPdfium, int indicePagina, string rutaDestino)
        {
            // Nota: documentoPdfium es PdfiumViewer.PdfDocument, no se usa aquí
            // Solo se mantiene por compatibilidad con la firma del método
            // La extracción real se hace leyendo el archivo original directamente con iText7

            // Este método se llama desde el contexto donde ya tenemos la ruta del archivo original
            // La extracción real se hace en ExtraerPaginaPDFNativa
        }

        /// <summary>
        /// Extrae una página específica de un PDF copiando el archivo completo y eliminando las demás páginas
        /// Este método es más robusto que copiar páginas individuales
        /// </summary>
        public static void ExtraerPaginaPorCopiaYEliminacion(string rutaArchivoOriginal, int numeroPagina, string rutaDestino)
        {
            iText.Kernel.Pdf.PdfReader? lector = null;
            iText.Kernel.Pdf.PdfWriter? escritor = null;
            iText.Kernel.Pdf.PdfDocument? documento = null;

            try
            {
                lector = new iText.Kernel.Pdf.PdfReader(rutaArchivoOriginal);
                escritor = new iText.Kernel.Pdf.PdfWriter(rutaDestino);
                documento = new iText.Kernel.Pdf.PdfDocument(lector, escritor);

                int totalPaginas = documento.GetNumberOfPages();

                // Verificar que la página existe
                if (numeroPagina < 1 || numeroPagina > totalPaginas)
                {
                    throw new ArgumentException($"Número de página {numeroPagina} fuera de rango. El documento tiene {totalPaginas} páginas.");
                }

                // Eliminar todas las páginas EXCEPTO la que queremos
                // Importante: eliminar de atrás hacia adelante para no afectar los índices
                for (int i = totalPaginas; i >= 1; i--)
                {
                    if (i != numeroPagina)
                    {
                        documento.RemovePage(i);
                    }
                }

                // Cerrar explícitamente en orden inverso
                documento.Close();
                documento = null;
                escritor.Close();
                escritor = null;
                lector.Close();
                lector = null;
            }
            catch (Exception ex)
            {
                // Asegurar que todos los recursos se liberen
                try { documento?.Close(); } catch { }
                try { escritor?.Close(); } catch { }
                try { lector?.Close(); } catch { }

                // Forzar garbage collection para liberar handles
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Intentar eliminar archivo parcial
                IntentarEliminarArchivo(rutaDestino);

                // Capturar excepción interna si existe
                string mensajeCompleto = ex.Message;
                if (ex.InnerException != null)
                {
                    mensajeCompleto += $" [Inner: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}]";
                }

                // Preservar tipo de excepción original para diagnóstico
                throw new Exception($"Error al extraer página {numeroPagina} por copia y eliminación: {ex.GetType().Name} - {mensajeCompleto}", ex);
            }
            finally
            {
                // Liberar recursos si no se hizo en try
                try { documento?.Close(); } catch { }
                try { escritor?.Close(); } catch { }
                try { lector?.Close(); } catch { }
            }
        }

        /// <summary>
        /// Rota 90° (u otros grados) un PDF de una sola página que ya está en disco,
        /// sumando la rotación indicada a la rotación actual del archivo. Se usa desde
        /// la pantalla de revisión manual de rotaciones para corregir páginas giradas
        /// incorrectamente por la detección automática.
        /// </summary>
        public static void RotarPaginaPdfEnDisco(string rutaPdf, int grados)
        {
            string rutaTemporal = rutaPdf + ".tmp";

            iText.Kernel.Pdf.PdfReader? lector = null;
            iText.Kernel.Pdf.PdfWriter? escritor = null;
            iText.Kernel.Pdf.PdfDocument? documento = null;

            try
            {
                lector = new iText.Kernel.Pdf.PdfReader(rutaPdf);
                escritor = new iText.Kernel.Pdf.PdfWriter(rutaTemporal);
                documento = new iText.Kernel.Pdf.PdfDocument(lector, escritor);

                var pagina = documento.GetPage(1);
                int rotacionActual = pagina.GetRotation();
                int nuevaRotacion = ((rotacionActual + grados) % 360 + 360) % 360;
                pagina.SetRotation(nuevaRotacion);

                documento.Close();
                documento = null;
                escritor.Close();
                escritor = null;
                lector.Close();
                lector = null;

                // Reemplazar el archivo original por la versión rotada
                File.Delete(rutaPdf);
                File.Move(rutaTemporal, rutaPdf);
            }
            catch (Exception ex)
            {
                try { documento?.Close(); } catch { }
                try { escritor?.Close(); } catch { }
                try { lector?.Close(); } catch { }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                IntentarEliminarArchivo(rutaTemporal);

                throw new Exception($"Error al rotar el PDF en disco '{rutaPdf}': {ex.GetType().Name} - {ex.Message}", ex);
            }
            finally
            {
                try { documento?.Close(); } catch { }
                try { escritor?.Close(); } catch { }
                try { lector?.Close(); } catch { }
            }
        }

        /// <summary>
        /// Extrae y rota una página copiando el archivo completo, eliminando las demás páginas y rotando la resultante
        /// </summary>
        public static void ExtraerYRotarPaginaPorCopiaYEliminacion(string rutaArchivoOriginal, int numeroPagina, string rutaDestino, int grados)
        {
            iText.Kernel.Pdf.PdfReader? lector = null;
            iText.Kernel.Pdf.PdfWriter? escritor = null;
            iText.Kernel.Pdf.PdfDocument? documento = null;

            try
            {
                lector = new iText.Kernel.Pdf.PdfReader(rutaArchivoOriginal);
                escritor = new iText.Kernel.Pdf.PdfWriter(rutaDestino);
                documento = new iText.Kernel.Pdf.PdfDocument(lector, escritor);

                int totalPaginas = documento.GetNumberOfPages();

                // Verificar que la página existe
                if (numeroPagina < 1 || numeroPagina > totalPaginas)
                {
                    throw new ArgumentException($"Número de página {numeroPagina} fuera de rango. El documento tiene {totalPaginas} páginas.");
                }

                // Eliminar todas las páginas EXCEPTO la que queremos
                for (int i = totalPaginas; i >= 1; i--)
                {
                    if (i != numeroPagina)
                    {
                        documento.RemovePage(i);
                    }
                }

                // Ahora solo queda una página (la primera, que era la numeroPagina)
                var pagina = documento.GetPage(1);
                int rotacionActual = pagina.GetRotation();
                int nuevaRotacion = (rotacionActual + grados) % 360;
                pagina.SetRotation(nuevaRotacion);

                // Cerrar explícitamente en orden inverso
                documento.Close();
                documento = null;
                escritor.Close();
                escritor = null;
                lector.Close();
                lector = null;
            }
            catch (Exception ex)
            {
                // Asegurar que todos los recursos se liberen
                try { documento?.Close(); } catch { }
                try { escritor?.Close(); } catch { }
                try { lector?.Close(); } catch { }

                // Forzar garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Intentar eliminar archivo parcial
                IntentarEliminarArchivo(rutaDestino);

                // Capturar excepción interna si existe
                string mensajeCompleto = ex.Message;
                if (ex.InnerException != null)
                {
                    mensajeCompleto += $" [Inner: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}]";
                }

                // Preservar tipo de excepción original para diagnóstico
                throw new Exception($"Error al extraer y rotar página {numeroPagina} por copia y eliminación: {ex.GetType().Name} - {mensajeCompleto}", ex);
            }
            finally
            {
                try { documento?.Close(); } catch { }
                try { escritor?.Close(); } catch { }
                try { lector?.Close(); } catch { }
            }
        }

        /// <summary>
        /// Extrae una página específica de un PDF usando iText7 (sin conversión a imagen)
        /// OBSOLETO: Usar ExtraerPaginaPorCopiaYEliminacion en su lugar
        /// </summary>
        [Obsolete("Usar ExtraerPaginaPorCopiaYEliminacion para mayor compatibilidad")]
        public static void ExtraerPaginaPDFNativa(string rutaArchivoOriginal, int numeroPagina, string rutaDestino)
        {
            try
            {
                using (var lector = new iText.Kernel.Pdf.PdfReader(rutaArchivoOriginal))
                using (var documentoOrigen = new iText.Kernel.Pdf.PdfDocument(lector))
                {
                    // Verificar que la página existe
                    if (numeroPagina < 1 || numeroPagina > documentoOrigen.GetNumberOfPages())
                    {
                        throw new ArgumentException($"Número de página {numeroPagina} fuera de rango. El documento tiene {documentoOrigen.GetNumberOfPages()} páginas.");
                    }

                    using (var escritor = new iText.Kernel.Pdf.PdfWriter(rutaDestino))
                    using (var documentoDestino = new iText.Kernel.Pdf.PdfDocument(escritor))
                    {
                        // Copiar la página específica (iText7 usa índice base 1)
                        var pagina = documentoOrigen.GetPage(numeroPagina);
                        pagina.CopyTo(documentoDestino);
                    }
                }
            }
            catch (Exception ex)
            {
                // Capturar excepción interna si existe
                string mensajeCompleto = ex.Message;
                if (ex.InnerException != null)
                {
                    mensajeCompleto += $" [Inner: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}]";
                }
                throw new Exception($"Error al extraer página {numeroPagina} del PDF con iText7: {ex.GetType().Name} - {mensajeCompleto}", ex);
            }
        }

        /// <summary>
        /// Extrae y rota una página específica de un PDF usando iText7
        /// </summary>
        public static void ExtraerYRotarPaginaPDF(string rutaArchivoOriginal, int numeroPagina, string rutaDestino, int grados)
        {
            try
            {
                using (var lector = new iText.Kernel.Pdf.PdfReader(rutaArchivoOriginal))
                using (var documentoOrigen = new iText.Kernel.Pdf.PdfDocument(lector))
                {
                    // Verificar que la página existe
                    if (numeroPagina < 1 || numeroPagina > documentoOrigen.GetNumberOfPages())
                    {
                        throw new ArgumentException($"Número de página {numeroPagina} fuera de rango. El documento tiene {documentoOrigen.GetNumberOfPages()} páginas.");
                    }

                    using (var escritor = new iText.Kernel.Pdf.PdfWriter(rutaDestino))
                    using (var documentoDestino = new iText.Kernel.Pdf.PdfDocument(escritor))
                    {
                        // Copiar la página
                        var pagina = documentoOrigen.GetPage(numeroPagina);
                        pagina.CopyTo(documentoDestino);

                        // Rotar la página copiada
                        var paginaDestino = documentoDestino.GetPage(1); // La página copiada es ahora la primera
                        int rotacionActual = paginaDestino.GetRotation();
                        int nuevaRotacion = (rotacionActual + grados) % 360;
                        paginaDestino.SetRotation(nuevaRotacion);
                    }
                }
            }
            catch (Exception ex)
            {
                // Capturar excepción interna si existe
                string mensajeCompleto = ex.Message;
                if (ex.InnerException != null)
                {
                    mensajeCompleto += $" [Inner: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}]";
                }
                throw new Exception($"Error al extraer y rotar página {numeroPagina} del PDF con iText7: {ex.GetType().Name} - {mensajeCompleto}", ex);
            }
        }

        /// <summary>
        /// Extrae una página usando renderizado a imagen de alta calidad (fallback cuando falla extracción nativa)
        /// </summary>
        private static void ExtraerPaginaPorImagen(PdfDocument documentoPdfium, int indicePagina, string rutaDestino, int rotacion = 0)
        {
            iText.Kernel.Pdf.PdfWriter? escritor = null;
            iText.Kernel.Pdf.PdfDocument? pdfDoc = null;
            iText.Layout.Document? document = null;

            try
            {
                // Renderizar a 300 DPI para mantener calidad razonable
                using (var imagen = documentoPdfium.Render(indicePagina, 300, 300, PdfRenderFlags.CorrectFromDpi))
                {
                    Image imagenFinal = imagen;

                    // Aplicar rotación si es necesaria
                    if (rotacion > 0)
                    {
                        imagenFinal = RotarImagen(imagen, rotacion);
                    }

                    try
                    {
                        // Convertir Image a byte array primero (antes de crear archivo destino)
                        byte[] imageBytes;
                        using (var ms = new System.IO.MemoryStream())
                        {
                            imagenFinal.Save(ms, ImageFormat.Jpeg);
                            imageBytes = ms.ToArray();
                        }

                        // Crear imagen de iText
                        var imageData = iText.IO.Image.ImageDataFactory.Create(imageBytes);
                        var pdfImage = new iText.Layout.Element.Image(imageData);

                        // Ahora sí crear el archivo PDF de salida
                        escritor = new iText.Kernel.Pdf.PdfWriter(rutaDestino);
                        pdfDoc = new iText.Kernel.Pdf.PdfDocument(escritor);
                        document = new iText.Layout.Document(pdfDoc);

                        // Ajustar tamaño de página al de la imagen
                        pdfDoc.SetDefaultPageSize(new iText.Kernel.Geom.PageSize(imagenFinal.Width, imagenFinal.Height));
                        pdfImage.SetFixedPosition(0, 0);
                        pdfImage.ScaleToFit(imagenFinal.Width, imagenFinal.Height);

                        document.Add(pdfImage);

                        // Cerrar explícitamente en orden inverso
                        document.Close();
                        document = null;
                        pdfDoc.Close();
                        pdfDoc = null;
                        escritor.Close();
                        escritor = null;
                    }
                    finally
                    {
                        // Liberar imagen rotada si es diferente de la original
                        if (imagenFinal != imagen)
                        {
                            imagenFinal.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Cerrar recursos explícitamente en caso de error
                try { document?.Close(); } catch { }
                try { pdfDoc?.Close(); } catch { }
                try { escritor?.Close(); } catch { }

                // Forzar liberación de recursos
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Eliminar archivo parcial si existe
                IntentarEliminarArchivo(rutaDestino);

                // Capturar excepción interna si existe
                string mensajeCompleto = ex.Message;
                if (ex.InnerException != null)
                {
                    mensajeCompleto += $" [Inner: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}]";
                }

                // Preservar el tipo de excepción original
                if (ex is iText.IO.Exceptions.IOException)
                {
                    throw new Exception($"Error de iText7 al crear PDF desde imagen: {ex.GetType().Name} - {mensajeCompleto}", ex);
                }
                else
                {
                    throw new Exception($"Error al extraer página por renderizado: {ex.GetType().Name} - {mensajeCompleto}", ex);
                }
            }
        }

        /// <summary>
        /// Guarda una imagen como archivo PDF (ya no se usa, mantenido por compatibilidad)
        /// </summary>
        [Obsolete("Usar ExtraerPaginaPDFNativa en su lugar")]
        private static void GuardarImagenComoPDF(Image imagen, string rutaDestino)
        {
            // Este método causaba archivos PDF inválidos y tamaños enormes
            // Mantenido solo por compatibilidad, no debería llamarse
            throw new NotSupportedException("Usar ExtraerPaginaPDFNativa para extraer páginas PDF nativas");
        }
    }
}
