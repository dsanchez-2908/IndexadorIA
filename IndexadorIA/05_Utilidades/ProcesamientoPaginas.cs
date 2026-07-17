using IndexadorIA.Entidades;
using PdfiumViewer;
using System.Drawing;
using System.Drawing.Imaging;

namespace IndexadorIA.Utilidades
{
    public class ProcesamientoPaginas
    {
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
        }

        /// <summary>
        /// Separa un archivo PDF en páginas individuales usando extracción nativa
        /// Maneja archivos corruptos de forma segura
        /// </summary>
        public static List<ResultadoPagina> SepararPaginasPDF(
            string rutaArchivo,
            string carpetaDestino,
            int secuenciaInicial,
            bool aplicarRotacion,
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
                            if (aplicarRotacion)
                            {
                                rotacion = DetectarRotacionNecesaria(imagen);
                                if (rotacion > 0)
                                {
                                    resultado.SeGiro = true;
                                    resultado.GradosRotacion = rotacion;
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
            bool aplicarRotacion,
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
                    if (aplicarRotacion)
                    {
                        var rotacion = DetectarRotacionNecesaria(imagen);
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
        /// Detecta la rotación necesaria basándose en la orientación de la imagen
        /// Heurística: Los planos generalmente tienen ancho > alto
        /// </summary>
        private static int DetectarRotacionNecesaria(Image imagen)
        {
            try
            {
                // Si el alto es mayor que el ancho, probablemente necesita rotación
                if (imagen.Height > imagen.Width)
                {
                    // Analizar distribución de contenido para determinar dirección
                    using (var bitmap = new Bitmap(imagen))
                    {
                        // Análisis simple: verificar densidad de contenido en bordes
                        int densidadIzquierda = AnalizarDensidadBorde(bitmap, BordeLado.Izquierda);
                        int densidadDerecha = AnalizarDensidadBorde(bitmap, BordeLado.Derecha);
                        int densidadSuperior = AnalizarDensidadBorde(bitmap, BordeLado.Superior);
                        int densidadInferior = AnalizarDensidadBorde(bitmap, BordeLado.Inferior);

                        // Si hay más contenido en los bordes verticales, rotar 90° a la derecha
                        int densidadVertical = densidadIzquierda + densidadDerecha;
                        int densidadHorizontal = densidadSuperior + densidadInferior;

                        if (densidadVertical > densidadHorizontal * 1.2)
                        {
                            // Rotar 90° en sentido horario
                            return 90;
                        }
                        else
                        {
                            // Rotar 270° (90° antihorario) como alternativa
                            return 270;
                        }
                    }
                }

                // No se requiere rotación
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private enum BordeLado { Izquierda, Derecha, Superior, Inferior }

        /// <summary>
        /// Analiza la densidad de píxeles no blancos en un borde de la imagen
        /// </summary>
        private static int AnalizarDensidadBorde(Bitmap bitmap, BordeLado lado)
        {
            int pixelesNoBlanccos = 0;
            int anchoBorde = 50; // Píxeles del borde a analizar

            switch (lado)
            {
                case BordeLado.Izquierda:
                    for (int y = 0; y < bitmap.Height; y += 5)
                    {
                        for (int x = 0; x < Math.Min(anchoBorde, bitmap.Width); x += 5)
                        {
                            if (EsPixelNoBlanco(bitmap.GetPixel(x, y)))
                                pixelesNoBlanccos++;
                        }
                    }
                    break;

                case BordeLado.Derecha:
                    for (int y = 0; y < bitmap.Height; y += 5)
                    {
                        for (int x = Math.Max(0, bitmap.Width - anchoBorde); x < bitmap.Width; x += 5)
                        {
                            if (EsPixelNoBlanco(bitmap.GetPixel(x, y)))
                                pixelesNoBlanccos++;
                        }
                    }
                    break;

                case BordeLado.Superior:
                    for (int y = 0; y < Math.Min(anchoBorde, bitmap.Height); y += 5)
                    {
                        for (int x = 0; x < bitmap.Width; x += 5)
                        {
                            if (EsPixelNoBlanco(bitmap.GetPixel(x, y)))
                                pixelesNoBlanccos++;
                        }
                    }
                    break;

                case BordeLado.Inferior:
                    for (int y = Math.Max(0, bitmap.Height - anchoBorde); y < bitmap.Height; y += 5)
                    {
                        for (int x = 0; x < bitmap.Width; x += 5)
                        {
                            if (EsPixelNoBlanco(bitmap.GetPixel(x, y)))
                                pixelesNoBlanccos++;
                        }
                    }
                    break;
            }

            return pixelesNoBlanccos;
        }

        private static bool EsPixelNoBlanco(Color pixel)
        {
            int brillo = (pixel.R + pixel.G + pixel.B) / 3;
            return brillo < 240;
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
