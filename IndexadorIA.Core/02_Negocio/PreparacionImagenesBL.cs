using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Utilidades;
using System.Drawing;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// Delegado para reportar progreso del procesamiento
    /// </summary>
    public delegate void ReportarProgresoHandler(int archivoActual, int totalArchivos, string mensaje);

    /// <summary>
    /// Business Logic para preparación de imágenes
    /// </summary>
    public class PreparacionImagenesBL
    {
        private readonly PreparacionImagenesDAL _dal;
        private readonly LogDAL _logDAL;

        public PreparacionImagenesBL()
        {
            _dal = new PreparacionImagenesDAL();
            _logDAL = new LogDAL();
        }

        /// <summary>
        /// Obtiene los lotes disponibles para preparación
        /// </summary>
        public List<LoteGridDto> ObtenerLotesParaPreparacion(string? filtroNombre = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null, bool incluirYaPreparados = false)
        {
            try
            {
                return _dal.ObtenerLotesParaPreparacion(filtroNombre, fechaDesde, fechaHasta, incluirYaPreparados);
            }
            catch (Exception ex)
            {
                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PreparacionImagenesBL",
                    DsMensaje = "Error al obtener lotes para preparación",
                    DsExcepcion = ex.ToString(),
                    DsUsuario = SesionActual.UsuarioActual?.DsNombreCompleto ?? "Sistema"
                });
                throw;
            }
        }

        /// <summary>
        /// Procesa los lotes seleccionados: convierte PDFs a JPG, recorta, genera Base64 y aplica OCR
        /// </summary>
        public void ProcesarLotesSeleccionados(
            List<LoteGridDto> lotesSeleccionados, 
            ConfiguracionRecorte configuracion, 
            ReportarProgresoHandler? reportarProgreso = null)
        {
            if (lotesSeleccionados == null || lotesSeleccionados.Count == 0)
                throw new ArgumentException("No se seleccionaron lotes para procesar");

            // Calcular total de archivos
            int totalArchivos = 0;
            foreach (var lote in lotesSeleccionados)
            {
                var archivos = _dal.ObtenerArchivosDeLote(lote.CdLote);
                totalArchivos += archivos.Count;
            }

            int archivosProcesados = 0;

            _logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.INFO,
                DsModulo = "PreparacionImagenesBL",
                DsMensaje = $"Iniciando procesamiento de {lotesSeleccionados.Count} lotes con {totalArchivos} archivos",
                DsExcepcion = $"DPI: {configuracion.DPI}, Recorte: {configuracion.PorcentajeHorizontal}% H x {configuracion.PorcentajeVertical}% V desde {configuracion.EsquinaRecorte}, OCR: {configuracion.EjecutarOCR}",
                DsUsuario = SesionActual.UsuarioActual?.DsNombreCompleto ?? "Sistema"
            });

            try
            {
                foreach (var lote in lotesSeleccionados)
                {
                    ProcesarLote(lote, configuracion, ref archivosProcesados, totalArchivos, reportarProgreso);
                }

                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "PreparacionImagenesBL",
                    DsMensaje = $"Procesamiento completado: {archivosProcesados} archivos procesados correctamente",
                    DsUsuario = SesionActual.UsuarioActual?.DsNombreCompleto ?? "Sistema"
                });
            }
            catch (Exception ex)
            {
                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PreparacionImagenesBL",
                    DsMensaje = "Error durante el procesamiento de lotes",
                    DsExcepcion = ex.ToString(),
                    DsUsuario = SesionActual.UsuarioActual?.DsNombreCompleto ?? "Sistema"
                });
                throw;
            }
        }

        /// <summary>
        /// Procesa un lote individual
        /// </summary>
        private void ProcesarLote(
            LoteGridDto lote, 
            ConfiguracionRecorte configuracion, 
            ref int archivosProcesados, 
            int totalArchivos, 
            ReportarProgresoHandler? reportarProgreso)
        {
            var archivos = _dal.ObtenerArchivosDeLote(lote.CdLote);
            var archivosConError = new List<string>();

            foreach (var archivo in archivos)
            {
                archivosProcesados++;

                try
                {
                    reportarProgreso?.Invoke(archivosProcesados, totalArchivos, 
                        $"Procesando {archivo.DsNombreArchivoPagina}...");

                    ProcesarArchivo(archivo, configuracion);

                    // Actualizar estado del archivo a "Imagen preparada" (cdEstado=3)
                    _dal.ActualizarEstadoArchivoPagina(archivo.CdArchivoPagina, 3);
                }
                catch (Exception ex)
                {
                    archivosConError.Add(archivo.DsNombreArchivoPagina);

                    _logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.ERROR,
                        DsModulo = "PreparacionImagenesBL",
                        DsMensaje = $"Error al procesar archivo {archivo.DsNombreArchivoPagina}",
                        DsExcepcion = ex.ToString(),
                        DsUsuario = SesionActual.UsuarioActual?.DsNombreCompleto ?? "Sistema"
                    });

                    // Continuar con el siguiente archivo
                    reportarProgreso?.Invoke(archivosProcesados, totalArchivos, 
                        $"ERROR en {archivo.DsNombreArchivoPagina}");
                }
            }

            if (archivosConError.Count > 0)
            {
                // NO marcar el lote como "Imágenes preparadas" (cdEstado=2): si se hiciera,
                // FrmProcesamientoIA lo tomaría como disponible para IA y fallaría al no
                // encontrar el .b64 de las páginas que quedaron sin procesar (ver bug reportado
                // con la página 1000 del lote 1). El lote queda pendiente para reintentar.
                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PreparacionImagenesBL",
                    DsMensaje = $"Lote {lote.CdLote} quedó INCOMPLETO: {archivosConError.Count} archivo(s) con error. " +
                                $"No se marca como 'Imágenes preparadas' para evitar que el procesamiento IA falle.",
                    DsExcepcion = string.Join(", ", archivosConError),
                    DsUsuario = SesionActual.UsuarioActual?.DsNombreCompleto ?? "Sistema"
                });
            }
            else
            {
                // Actualizar estado del lote a "Imágenes preparadas" (cdEstado=2)
                _dal.ActualizarEstadoLote(lote.CdLote, 2);
            }
        }

        /// <summary>
        /// Procesa un archivo individual: PDF→JPG, recorte, Base64, OCR
        /// </summary>
        private void ProcesarArchivo(ArchivoPagina archivo, ConfiguracionRecorte configuracion)
        {
            if (!File.Exists(archivo.DsRutaCompleta))
                throw new FileNotFoundException($"No se encontró el archivo: {archivo.DsRutaCompleta}");

            Bitmap? bitmapOriginal = null;
            Bitmap? bitmapRecortado = null;

            try
            {
                // 1. Convertir PDF a Bitmap
                bitmapOriginal = ProcesamientoImagenes.ConvertirPDFaBitmap(archivo.DsRutaCompleta, configuracion.DPI);

                if (bitmapOriginal == null)
                    throw new Exception("No se pudo convertir el PDF a imagen");

                // 2. Recortar la imagen
                bitmapRecortado = ProcesamientoImagenes.RecortarImagen(
                    bitmapOriginal, 
                    configuracion.EsquinaRecorte, 
                    configuracion.PorcentajeVertical, 
                    configuracion.PorcentajeHorizontal);

                // 3. Guardar como JPG (mismo nombre que el PDF, misma carpeta)
                string rutaJPG = Path.ChangeExtension(archivo.DsRutaCompleta, ".jpg");
                ProcesamientoImagenes.GuardarJPG(bitmapRecortado, rutaJPG, configuracion.CalidadJPG);

                // 4. Convertir a Base64 y guardar en archivo .b64
                string base64String = ProcesamientoImagenes.ConvertirImagenABase64(rutaJPG);
                string rutaBase64 = Path.ChangeExtension(archivo.DsRutaCompleta, ".b64");
                ProcesamientoImagenes.GuardarBase64EnArchivo(base64String, rutaBase64);

                // 5. Aplicar OCR si está habilitado
                if (configuracion.EjecutarOCR)
                {
                    string textoOCR = AplicarOCR(rutaJPG);
                    _dal.InsertarResultadoOCR(archivo.CdArchivoPagina, textoOCR);
                }
            }
            finally
            {
                // Liberar recursos
                bitmapOriginal?.Dispose();
                bitmapRecortado?.Dispose();
            }
        }

        /// <summary>
        /// Aplica OCR a una imagen JPG usando Tesseract
        /// </summary>
        private string AplicarOCR(string rutaImagen)
        {
            try
            {
                // Obtener ruta de tessdata
                string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
                string rutaTessdata = Path.Combine(directorioEjecucion, "tessdata");

                // Verificar que exista la carpeta tessdata
                if (!Directory.Exists(rutaTessdata))
                {
                    throw new DirectoryNotFoundException(
                        $"No se encontró la carpeta tessdata en: {rutaTessdata}. " +
                        "Debe colocar los archivos de idioma de Tesseract en esta carpeta.");
                }

                // Inicializar motor de Tesseract con idioma español
                using (var engine = new Tesseract.TesseractEngine(rutaTessdata, "spa", Tesseract.EngineMode.Default))
                {
                    // Cargar la imagen
                    using (var img = Tesseract.Pix.LoadFromFile(rutaImagen))
                    {
                        // Procesar OCR
                        using (var page = engine.Process(img))
                        {
                            string texto = page.GetText();
                            float confianza = page.GetMeanConfidence();

                            _logDAL.Insertar(new LogRegistro
                            {
                                DsNivel = LogRegistro.Niveles.INFO,
                                DsModulo = "PreparacionImagenesBL - OCR",
                                DsMensaje = $"OCR aplicado a {Path.GetFileName(rutaImagen)}",
                                DsExcepcion = $"Confianza: {confianza:P2}, Caracteres: {texto.Length}",
                                DsUsuario = SesionActual.UsuarioActual?.DsNombreCompleto ?? "Sistema"
                            });

                            return texto;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.WARNING,
                    DsModulo = "PreparacionImagenesBL - OCR",
                    DsMensaje = $"Error al aplicar OCR a {Path.GetFileName(rutaImagen)}",
                    DsExcepcion = ex.ToString(),
                    DsUsuario = SesionActual.UsuarioActual?.DsNombreCompleto ?? "Sistema"
                });

                return $"[Error OCR: {ex.Message}]";
            }
        }
    }
}
