using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Data.SqlClient;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// L�gica de negocio para procesamiento de lotes con OpenAI Batch API
    /// </summary>
    public static class OpenAIBL
    {
        // Limite real de OpenAI para gpt-4o-2024-08-06: 209.715.200 bytes (200MB) por archivo de batch
        // (error "maximum_input_file_size_exceeded"). Se deja margen de seguridad para el overhead de JSON/prompt.
        private const long TAMANIO_MAXIMO_ARCHIVO_BATCH_BYTES = 180L * 1024 * 1024;

        // Timeout ampliado: con archivos JSONL de cientos de MB (batches grandes con imagenes en Base64),
        // el timeout por defecto de 100s de HttpClient se agota durante la subida y aborta la operacion
        // aunque la transferencia siga en curso. Ver OpenAIBL.SubirArchivoAsync.
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(20) };

        /// <summary>
        /// Normaliza un texto para comparaciones: quita acentos, espacios extra y pasa a mayusculas.
        /// </summary>
        private static string NormalizarTexto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return string.Empty;
            }

            string textoSinAcentos = QuitarAcentos(texto.Trim());
            return textoSinAcentos.ToUpperInvariant();
        }

        /// <summary>
        /// Limpia un texto extraido por la IA, dejando null si queda vacio tras el recorte.
        /// </summary>
        private static string? LimpiarTexto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return null;
            }

            string limpio = texto.Trim();
            return limpio.Length == 0 ? null : limpio;
        }

        /// <summary>
        /// Quita diacriticos (acentos) de un texto.
        /// </summary>
        private static string QuitarAcentos(string texto)
        {
            string normalizado = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalizado)
            {
                var categoria = CharUnicodeInfo.GetUnicodeCategory(c);
                if (categoria != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Normaliza el campo dsExpediente con formato [EX]-[ANIO]-[EXPEDIENTE]-[GCABA]-[REPARTICION].
        /// Si no se puede parsear correctamente en 5 partes, devuelve el texto original (recortado)
        /// y marca <paramref name="esValido"/> en false para que la confianza se fije en 0.5000.
        /// </summary>
        private static string? NormalizarExpediente(string? dsExpediente, List<Reparticion> reparticiones, out bool esValido)
        {
            esValido = false;

            if (string.IsNullOrWhiteSpace(dsExpediente))
            {
                return null;
            }

            string[] partes = dsExpediente.Split('-', StringSplitOptions.None);

            if (partes.Length != 5)
            {
                return dsExpediente.Trim();
            }

            string ex = partes[0].Trim();
            string anioTexto = partes[1].Trim();
            string expedienteTexto = partes[2].Trim();
            string gcaba = partes[3].Trim();
            string reparticionTexto = partes[4].Trim();

            if (!int.TryParse(anioTexto, NumberStyles.Integer, CultureInfo.InvariantCulture, out int anio)
                || anio < 1900 || anio > 2050)
            {
                return dsExpediente.Trim();
            }

            if (!int.TryParse(expedienteTexto, NumberStyles.Integer, CultureInfo.InvariantCulture, out int numeroExpediente))
            {
                return dsExpediente.Trim();
            }

            string anioNormalizado = anio.ToString("D4", CultureInfo.InvariantCulture);
            string expedienteNormalizado = numeroExpediente.ToString("D8", CultureInfo.InvariantCulture);

            string reparticionNormalizada = NormalizarTexto(reparticionTexto);
            var reparticionEncontrada = reparticiones
                .FirstOrDefault(r => NormalizarTexto(r.DsReparticion) == reparticionNormalizada);

            if (reparticionEncontrada == null)
            {
                return $"{ex}-{anioNormalizado}-{expedienteNormalizado}-{gcaba}-{reparticionTexto}";
            }

            esValido = true;
            return $"{ex}-{anioNormalizado}-{expedienteNormalizado}-{gcaba}-{reparticionEncontrada.DsReparticion}";
        }

        /// <summary>
        /// Normaliza el campo dsDireccion: mayusculas, quita � , . y reemplaza / por &#8211;
        /// </summary>
        private static string? NormalizarDireccion(string? dsDireccion)
        {
            if (string.IsNullOrWhiteSpace(dsDireccion))
            {
                return null;
            }

            string resultado = dsDireccion.Trim().ToUpperInvariant();
            resultado = resultado.Replace("�", string.Empty)
                                  .Replace(",", string.Empty)
                                  .Replace(".", string.Empty)
                                  .Replace("/", "\u2013");

            return resultado;
        }

        /// <summary>
        /// Normaliza el campo dsSeccion: quita espacios/caracteres especiales y completa con ceros a 3 digitos.
        /// </summary>
        private static string? NormalizarSeccion(string? dsSeccion)
        {
            if (string.IsNullOrWhiteSpace(dsSeccion))
            {
                return null;
            }

            string soloAlfanumerico = Regex.Replace(dsSeccion, @"[^A-Za-z0-9]", string.Empty);

            if (soloAlfanumerico.Length == 0)
            {
                return null;
            }

            if (int.TryParse(soloAlfanumerico, NumberStyles.Integer, CultureInfo.InvariantCulture, out int numeroSeccion))
            {
                return numeroSeccion.ToString("D3", CultureInfo.InvariantCulture);
            }

            return soloAlfanumerico.ToUpperInvariant();
        }

        /// <summary>
        /// Normaliza el campo dsManzana: quita espacios/caracteres especiales; si es solo numerico
        /// completa a 3 digitos; si tiene parte alfabetica "LL" completa la parte numerica a 2 digitos
        /// y deja "LL" en mayuscula; si tiene otra parte alfabetica completa la parte numerica a 3 digitos
        /// y deja el texto en mayuscula.
        /// </summary>
        private static string? NormalizarManzana(string? dsManzana)
        {
            return NormalizarAlfanumericoConLetra(dsManzana, letraMayuscula: true);
        }

        /// <summary>
        /// Normaliza el campo dsParcela: igual que Manzana, pero la parte alfabetica (si no es "LL")
        /// se deja en minuscula.
        /// </summary>
        private static string? NormalizarParcela(string? dsParcela)
        {
            return NormalizarAlfanumericoConLetra(dsParcela, letraMayuscula: false);
        }

        /// <summary>
        /// Logica compartida de normalizacion para Manzana/Parcela.
        /// </summary>
        private static string? NormalizarAlfanumericoConLetra(string? valor, bool letraMayuscula)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return null;
            }

            string soloAlfanumerico = Regex.Replace(valor, @"[^A-Za-z0-9]", string.Empty);

            if (soloAlfanumerico.Length == 0)
            {
                return null;
            }

            // Caso solo numerico
            if (int.TryParse(soloAlfanumerico, NumberStyles.Integer, CultureInfo.InvariantCulture, out int numeroSolo))
            {
                return numeroSolo.ToString("D3", CultureInfo.InvariantCulture);
            }

            string parteNumerica = new string(soloAlfanumerico.Where(char.IsDigit).ToArray());
            string parteTexto = new string(soloAlfanumerico.Where(c => !char.IsDigit(c)).ToArray());

            if (parteNumerica.Length == 0)
            {
                return soloAlfanumerico.ToUpperInvariant();
            }

            // Si la parte numérica es demasiado larga para un int (dato mal reconocido por la IA),
            // no formatear como número: devolver el valor tal cual, sin intentar rellenar con ceros.
            if (!int.TryParse(parteNumerica, NumberStyles.Integer, CultureInfo.InvariantCulture, out int numero))
            {
                return letraMayuscula
                    ? soloAlfanumerico.ToUpperInvariant()
                    : soloAlfanumerico;
            }

            bool esLL = parteTexto.Equals("LL", StringComparison.OrdinalIgnoreCase);

            if (esLL)
            {
                return $"{numero.ToString("D2", CultureInfo.InvariantCulture)}LL";
            }

            string parteTextoFormateada = letraMayuscula
                ? parteTexto.ToUpperInvariant()
                : parteTexto.ToLowerInvariant();

            return $"{numero.ToString("D3", CultureInfo.InvariantCulture)}{parteTextoFormateada}";
        }

        /// <summary>
        /// Procesa un lote completo con OpenAI Batch API
        /// </summary>
        public static async Task<bool> ProcesarLoteAsync(
            int cdLote,
            int cdProyecto,
            int cdUsuario,
            IProgress<string>? progreso = null,
            CancellationToken cancellationToken = default)
        {
            var loteDAL = new LoteDAL();
            var logDAL = new LogDAL();

            try
            {
                progreso?.Report($"Iniciando procesamiento del lote {cdLote}");

                // Cambiar estado del lote a "Procesando (3)"
                loteDAL.ActualizarEstado(cdLote, 3, cdUsuario);

                // 1. Obtener el prompt configurado
                progreso?.Report("Obteniendo prompt configurado");
                var promptDAL = new PromptDAL();
                var prompt = promptDAL.ObtenerPorProyecto(cdProyecto);

                if (prompt == null)
                {
                    throw new Exception("No se encontr� un prompt configurado para el proyecto");
                }

                // 2. Obtener las p�ginas del lote
                progreso?.Report("Obteniendo p�ginas del lote");
                var paginas = loteDAL.ObtenerArchivosPaginasPorLote(cdLote);

                if (paginas.Count == 0)
                {
                    throw new Exception("No se encontraron p�ginas para procesar en el lote");
                }


                // 3. Trocear el lote en sub-lotes cuyo archivo JSONL no supere el limite de OpenAI
                // (209.715.200 bytes / 200MB para gpt-4o-2024-08-06, ver error "maximum_input_file_size_exceeded").
                // Se usa un margen de seguridad (180MB) porque el JSONL final agrega overhead de JSON/prompt
                // sobre el tamano puro de los .b64.
                var subLotes = DividirEnSubLotesPorTamano(paginas, TAMANIO_MAXIMO_ARCHIVO_BATCH_BYTES);

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarLoteAsync",
                    DsMensaje = $"Lote {cdLote}: {paginas.Count} paginas divididas en {subLotes.Count} sub-lote(s) por tamano"
                });

                for (int i = 0; i < subLotes.Count; i++)
                {
                    var subPaginas = subLotes[i];
                    string subLoteDesc = subLotes.Count > 1 ? $" (sub-lote {i + 1}/{subLotes.Count})" : "";

                    // 3.1 Crear archivo JSONL para el sub-lote
                    progreso?.Report($"Creando archivo batch con {subPaginas.Count} paginas{subLoteDesc}");
                    string batchFilePath = await CrearArchivoBatchAsync(subPaginas, prompt.DsPrompt, cdProyecto);

                    // 3.2 Subir archivo a OpenAI
                    progreso?.Report($"Subiendo archivo a OpenAI{subLoteDesc}");
                    string fileId = await SubirArchivoAsync(batchFilePath);

                    // Esperar a que OpenAI termine de procesar el archivo subido antes de crear
                    // el batch job. Con archivos grandes, el file_id puede tardar unos segundos
                    // en quedar disponible (consistencia eventual del lado de OpenAI); si se crea
                    // el batch antes de tiempo, OpenAI responde "Cannot find file ... or organization
                    // does not have access to it" y el batch queda en estado failed.
                    progreso?.Report($"Esperando que OpenAI procese el archivo subido{subLoteDesc}");
                    await EsperarArchivoProcesadoAsync(fileId);

                    // 3.3/3.4/3.5 Crear el batch job y esperar resultados. Aun con el archivo en estado
                    // "processed", el servicio de Batches de OpenAI puede tardar mas en verlo disponible
                    // (consistencia eventual entre servicios internos), lo que produce el mismo error
                    // "Cannot find file ... or organization does not have access to it" pero recien al
                    // validar el batch (no al crearlo). Como un batch fallido no puede reintentarse,
                    // se crea un batch NUEVO tras una espera adicional si se detecta este error puntual.
                    const int maxIntentosBatch = 3;
                    string batchId;
                    string resultFileId;
                    for (int intentoBatch = 1; ; intentoBatch++)
                    {
                        progreso?.Report($"Creando batch job en OpenAI{subLoteDesc} (intento {intentoBatch}/{maxIntentosBatch})");
                        batchId = await CrearBatchJobAsync(fileId);

                        GuardarBatchTracking(cdLote, batchId, fileId);

                        progreso?.Report($"Esperando resultados del batch {batchId}{subLoteDesc} (esto puede tardar varios minutos)");
                        try
                        {
                            resultFileId = await EsperarResultadosAsync(batchId, progreso, cancellationToken);
                            break;
                        }
                        catch (Exception ex) when (
                            intentoBatch < maxIntentosBatch &&
                            ex.Message.Contains("Cannot find file", StringComparison.OrdinalIgnoreCase))
                        {
                            int esperaSegundos = 15 * intentoBatch;
                            progreso?.Report($"OpenAI aun no reconoce el archivo para Batches. Reintentando en {esperaSegundos}s{subLoteDesc}");
                            await Task.Delay(TimeSpan.FromSeconds(esperaSegundos), cancellationToken);
                        }
                    }

                    // 3.6 Procesar resultados
                    progreso?.Report($"Descargando y procesando resultados{subLoteDesc}");
                    await ProcesarResultadosAsync(resultFileId, subPaginas, cdLote, cdProyecto, cdUsuario);

                    // Limpiar archivo temporal del sub-lote
                    if (File.Exists(batchFilePath))
                    {
                        File.Delete(batchFilePath);
                    }
                }

                // 4. Cambiar estado del lote a "Procesado IA (4)"
                loteDAL.ActualizarEstado(cdLote, 4, cdUsuario);

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarLoteAsync",
                    DsMensaje = $"Lote {cdLote} procesado exitosamente con OpenAI"
                });

                progreso?.Report($"Lote {cdLote} procesado exitosamente");
                return true;
            }
            catch (TimeoutException timeoutEx)
            {
                // El batch fue enviado exitosamente pero a�n est� proces�ndose
                // NO revertir el estado del lote - dejarlo en estado 3 (Procesando)

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarLoteAsync",
                    DsMensaje = $"Lote {cdLote} enviado a OpenAI. Procesamiento en segundo plano. {timeoutEx.Message}"
                });

                progreso?.Report($"Lote enviado a OpenAI - Procesando en segundo plano");
                return true; // Retornar true porque el batch S� fue creado exitosamente
            }
            catch (HttpRequestException httpEx)
            {
                // Error de red/conexi�n DESPU�S de que el batch fue creado
                // Si llegamos aqu� despu�s de GuardarBatchTracking, el batch S� existe en OpenAI
                // NO revertir el estado del lote - dejarlo en estado 3 (Procesando)

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.WARNING,
                    DsModulo = "OpenAIBL.ProcesarLoteAsync",
                    DsMensaje = $"Lote {cdLote} enviado a OpenAI pero ocurri� error de red al consultar estado. El batch contin�a proces�ndose en segundo plano. Error: {httpEx.Message}"
                });

                progreso?.Report($"Lote enviado a OpenAI - Error de red al consultar estado, pero el batch sigue proces�ndose");
                return true; // Retornar true porque el batch fue creado exitosamente
            }
            catch (OperationCanceledException)
            {
                // Revertir lote a estado "Listo para IA (2)"
                loteDAL.ActualizarEstado(cdLote, 2, cdUsuario);

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarLoteAsync",
                    DsMensaje = $"Procesamiento del lote {cdLote} cancelado por el usuario"
                });

                throw;
            }
            catch (Exception ex)
            {
                // Revertir lote a estado "Listo para IA (2)" en caso de error
                loteDAL.ActualizarEstado(cdLote, 2, cdUsuario);

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "OpenAIBL.ProcesarLoteAsync",
                    DsMensaje = $"Error al procesar lote {cdLote}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });

                progreso?.Report($"Error al procesar lote {cdLote}");
                return false;
            }
        }

        /// <summary>
        /// Divide la lista de paginas en sub-lotes cuyo tamano acumulado de archivos .b64 no supere
        /// maxBytesPorSubLote, para evitar el error "maximum_input_file_size_exceeded" de OpenAI
        /// cuando el lote completo genera un JSONL demasiado grande.
        /// </summary>
        private static List<List<ArchivoPagina>> DividirEnSubLotesPorTamano(List<ArchivoPagina> paginas, long maxBytesPorSubLote)
        {
            var resultado = new List<List<ArchivoPagina>>();
            var subLoteActual = new List<ArchivoPagina>();
            long tamanioAcumulado = 0;

            foreach (var pagina in paginas)
            {
                long tamanioPagina = 0;
                if (!string.IsNullOrEmpty(pagina.RutaBase64) && File.Exists(pagina.RutaBase64))
                {
                    tamanioPagina = new FileInfo(pagina.RutaBase64).Length;
                }

                if (subLoteActual.Count > 0 && tamanioAcumulado + tamanioPagina > maxBytesPorSubLote)
                {
                    resultado.Add(subLoteActual);
                    subLoteActual = new List<ArchivoPagina>();
                    tamanioAcumulado = 0;
                }

                subLoteActual.Add(pagina);
                tamanioAcumulado += tamanioPagina;
            }

            if (subLoteActual.Count > 0)
            {
                resultado.Add(subLoteActual);
            }

            return resultado;
        }

        private static async Task<string> CrearArchivoBatchAsync(List<ArchivoPagina> paginas, string promptTemplate, int cdProyecto)
        {
            var jsonlLines = new List<string>();
            var logDAL = new LogDAL();

            // Validaci�n previa: si falta alg�n .b64 (p.ej. el lote qued� marcado como "preparado"
            // pero alguna p�gina fall� en PreparacionImagenesBL), fallar r�pido con el listado
            // completo de p�ginas faltantes en vez de descubrirlo reci�n al llegar a esa p�gina
            // luego de haber generado/serializado el resto del lote (desperdicio de CPU/memoria).
            var paginasFaltantes = paginas
                .Where(p => string.IsNullOrEmpty(p.RutaBase64) || !File.Exists(p.RutaBase64))
                .Select(p => p.CdArchivoPagina)
                .ToList();

            if (paginasFaltantes.Count > 0)
            {
                string mensaje = $"Faltan {paginasFaltantes.Count} archivo(s) Base64 de {paginas.Count} p�ginas del lote. " +
                    $"P�ginas: {string.Join(", ", paginasFaltantes.Take(20))}" +
                    (paginasFaltantes.Count > 20 ? ", ..." : "");

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                    DsMensaje = mensaje
                });

                throw new Exception(mensaje + ". Vuelva a ejecutar la preparaci�n de im�genes para este lote.");
            }

            foreach (var pagina in paginas)
            {
                // Log de diagn�stico: ruta del archivo Base64
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                    DsMensaje = $"P�gina {pagina.CdArchivoPagina}: Ruta Base64 = {pagina.RutaBase64}"
                });

                // Leer Base64 desde archivo
                if (string.IsNullOrEmpty(pagina.RutaBase64) || !File.Exists(pagina.RutaBase64))
                {
                    throw new Exception($"No se encontr� el archivo Base64 para la p�gina {pagina.CdArchivoPagina}");
                }

                string base64Content = await File.ReadAllTextAsync(pagina.RutaBase64);

                // Log del tama�o del Base64
                long base64SizeBytes = base64Content.Length;
                long base64SizeMB = base64SizeBytes / (1024 * 1024);

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                    DsMensaje = $"P�gina {pagina.CdArchivoPagina}: Tama�o Base64 = {base64SizeBytes:N0} bytes ({base64SizeMB} MB)"
                });

                // Validar tama�o (OpenAI limita a 20MB por imagen)
                const long MAX_SIZE_BYTES = 20 * 1024 * 1024; // 20 MB
                string imageDetail = "auto"; // Por defecto "auto"

                if (base64SizeBytes > MAX_SIZE_BYTES)
                {
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.WARNING,
                        DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                        DsMensaje = $"ADVERTENCIA: P�gina {pagina.CdArchivoPagina} excede 20MB ({base64SizeMB} MB). Esto puede causar errores 500 en OpenAI."
                    });
                    imageDetail = "low"; // Forzar detalle bajo para im�genes grandes
                }
                else if (base64SizeBytes > 5 * 1024 * 1024) // Mayor a 5MB
                {
                    // Para im�genes entre 5MB y 20MB, usar detalle bajo por seguridad
                    imageDetail = "low";
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.INFO,
                        DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                        DsMensaje = $"P�gina {pagina.CdArchivoPagina}: Usando detail='low' debido al tama�o ({base64SizeMB} MB)"
                    });
                }

                // Usar el prompt tal cual (sin reemplazos por ahora, se pueden agregar despu�s)
                string promptFinal = promptTemplate;

                // Crear objeto de request seg�n formato Batch API
                var batchItem = new
                {
                    custom_id = $"page_{pagina.CdArchivoPagina}",
                    method = "POST",
                    url = "/v1/chat/completions",
                    body = new
                    {
                        model = "gpt-4o-2024-08-06",  // Modelo con fecha espec�fica (requerido para Batch API)
                        messages = new[]
                        {
                            new
                            {
                                role = "user",
                                content = new object[]
                                {
                                    new { type = "text", text = promptFinal },
                                    new
                                    {
                                        type = "image_url",
                                        image_url = new
                                        {
                                            url = $"data:image/png;base64,{base64Content}",
                                            detail = imageDetail  // "low", "auto", o "high"
                                        }
                                    }
                                }
                            }
                        },
                        max_tokens = 2000,
                        response_format = new { type = "json_object" }
                    }
                };

                // Serializar sin UnsafeRelaxedJsonEscaping para que escape correctamente \r\n
                string jsonLine = JsonSerializer.Serialize(batchItem);
                jsonlLines.Add(jsonLine);
            }

            // Guardar archivo JSONL temporal con encoding UTF-8 sin BOM y con \n (Unix)
            string tempPath = Path.Combine(Path.GetTempPath(), $"batch_{cdProyecto}_{Guid.NewGuid()}.jsonl");

            // UTF-8 sin BOM
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            // Escribir cada l�nea con \n (Unix line ending) en lugar de \r\n (Windows)
            await File.WriteAllTextAsync(tempPath, string.Join("\n", jsonlLines) + "\n", utf8WithoutBom);

            // Log del contenido para diagn�stico (solo primera l�nea)
            if (jsonlLines.Count > 0)
            {
                string primeraLinea = jsonlLines[0];
                // Truncar si es muy largo
                if (primeraLinea.Length > 500)
                    primeraLinea = primeraLinea.Substring(0, 500) + "... (truncado)";

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                    DsMensaje = $"Archivo JSONL creado con {jsonlLines.Count} l�neas. Primera l�nea: {primeraLinea}"
                });
            }

            return tempPath;
        }

        private static async Task<string> SubirArchivoAsync(string filePath)
        {
            var parametroDAL = new ParametrosDAL();
            string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY") 
                ?? throw new Exception("No se encontr� la API Key de OpenAI en par�metros");
                string? dsModelo = parametroDAL.ObtenerValor("OPENAI_MODEL");

            // Log de diagn�stico PRE-subida
            var logDAL = new LogDAL();
            var fileInfo = new FileInfo(filePath);
            string fileName = Path.GetFileName(filePath);

            logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.INFO,
                DsModulo = "OpenAIBL.SubirArchivoAsync",
                DsMensaje = $"Preparando subida - Archivo: {fileName}, Tama�o: {fileInfo.Length} bytes, Existe: {fileInfo.Exists}"
            });

            // Diagnostico liviano: solo se lee la primera linea (sin cargar los ~646MB completos en memoria,
            // como hacia File.ReadAllLinesAsync/ReadAllBytesAsync previamente).
            int lineCount = 0;
            string primeraLinea = "VAC�O";
            using (var reader = new StreamReader(filePath))
            {
                string? linea = await reader.ReadLineAsync();
                if (linea != null)
                {
                    primeraLinea = linea.Substring(0, Math.Min(200, linea.Length));
                    lineCount = 1;
                    while (await reader.ReadLineAsync() != null)
                    {
                        lineCount++;
                    }
                }
            }

            logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.INFO,
                DsModulo = "OpenAIBL.SubirArchivoAsync",
                DsMensaje = $"Contenido JSONL - L�neas: {lineCount}, Primera l�nea (primeros 200 chars): {primeraLinea}"
            });

            using var formContent = new MultipartFormDataContent();

            // Subida en streaming directo desde disco: evita duplicar el archivo completo en memoria
            // (antes: ByteArrayContent + File.ReadAllBytesAsync cargaba ~646MB adicionales en RAM).
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 81920, useAsync: true);
            var fileContent = new StreamContent(fileStream);
            // OpenAI batch API espera text/plain para archivos .jsonl
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

            formContent.Add(fileContent, "file", fileName);
            formContent.Add(new StringContent("batch"), "purpose");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/files");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = formContent;

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Log de respuesta
            logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.INFO,
                DsModulo = "OpenAIBL.SubirArchivoAsync",
                DsMensaje = $"Response subir archivo - Status: {response.StatusCode}, Content: {responseContent}"
            });

            response.EnsureSuccessStatusCode();

            var json = JsonDocument.Parse(responseContent);

            return json.RootElement.GetProperty("id").GetString() 
                ?? throw new Exception("No se pudo obtener el ID del archivo subido");
        }

        /// <summary>
        /// Consulta GET /v1/files/{fileId} en un bucle con esperas cortas hasta que OpenAI
        /// reporte el archivo como "processed" (o un estado terminal), para evitar crear el
        /// batch job contra un file_id que todavia no esta disponible para la organizacion
        /// (error tipico: "Cannot find file ..., or organization ... does not have access to it").
        /// </summary>
        private static async Task EsperarArchivoProcesadoAsync(string fileId)
        {
            var parametroDAL = new ParametrosDAL();
            string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY")
                ?? throw new Exception("No se encontro la API Key de OpenAI en parametros");

            var logDAL = new LogDAL();
            const int maxIntentos = 10;
            const int esperaMs = 2000;

            for (int intento = 1; intento <= maxIntentos; intento++)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.openai.com/v1/files/{fileId}");
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonDocument.Parse(responseContent);
                    string? estado = json.RootElement.TryGetProperty("status", out var statusProp)
                        ? statusProp.GetString()
                        : null;

                    if (string.Equals(estado, "processed", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    if (string.Equals(estado, "error", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception($"OpenAI reporto error al procesar el archivo {fileId}: {responseContent}");
                    }
                }

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.EsperarArchivoProcesadoAsync",
                    DsMensaje = $"Intento {intento}/{maxIntentos} - fileId={fileId}, Status HTTP={response.StatusCode}, Content={responseContent}"
                });

                await Task.Delay(esperaMs);
            }

            // Si tras los reintentos no se confirmo el estado "processed", se continua igualmente:
            // puede que OpenAI ya lo tenga disponible pero no reporte el campo status como se espera.
            // El error real (si lo hay) se vera al crear el batch job.
            logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.WARNING,
                DsModulo = "OpenAIBL.EsperarArchivoProcesadoAsync",
                DsMensaje = $"No se confirmo el estado 'processed' del archivo {fileId} tras {maxIntentos} intentos. Se continua con la creacion del batch job."
            });
        }

        private static async Task<string> CrearBatchJobAsync(string fileId)
        {
            var parametroDAL = new ParametrosDAL();
            string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY") 
                ?? throw new Exception("No se encontr� la API Key de OpenAI en par�metros");
                string? dsModelo = parametroDAL.ObtenerValor("OPENAI_MODEL");

            var requestBody = new
            {
                input_file_id = fileId,
                endpoint = "/v1/chat/completions",
                completion_window = "24h"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/batches");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Log de diagn�stico
            var logDAL = new LogDAL();
            logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.INFO,
                DsModulo = "OpenAIBL.CrearBatchJobAsync",
                DsMensaje = $"Response crear batch - Status: {response.StatusCode}, Content: {responseContent}"
            });

            response.EnsureSuccessStatusCode();

            var json = JsonDocument.Parse(responseContent);

            return json.RootElement.GetProperty("id").GetString() 
                ?? throw new Exception("No se pudo obtener el ID del batch");
        }

        private static async Task<string> EsperarResultadosAsync(
            string batchId,
            IProgress<string>? progreso,
            CancellationToken cancellationToken)
        {
            var parametroDAL = new ParametrosDAL();
            string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY") 
                ?? throw new Exception("No se encontr� la API Key de OpenAI en par�metros");
                string? dsModelo = parametroDAL.ObtenerValor("OPENAI_MODEL");

            var logDAL = new LogDAL();
            int intentos = 0;
            const int maxIntentos = 60; // 5 minutos (5 seg * 60) - para pruebas

            while (intentos < maxIntentos)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.openai.com/v1/batches/{batchId}");
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

                var response = await _httpClient.SendAsync(request, cancellationToken);

                // Registrar toda la respuesta para diagn�stico
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                // Log para diagn�stico - solo en los primeros 3 intentos
                if (intentos < 3)
                {
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.INFO,
                        DsModulo = "OpenAIBL.EsperarResultadosAsync",
                        DsMensaje = $"Respuesta batch {batchId} (intento {intentos + 1}): {responseContent}"
                    });
                }

                response.EnsureSuccessStatusCode();
                var json = JsonDocument.Parse(responseContent);

                string status = json.RootElement.GetProperty("status").GetString() ?? "unknown";
                int tiempoTranscurrido = (intentos + 1) * 5; // segundos
                int minutos = tiempoTranscurrido / 60;
                int segundos = tiempoTranscurrido % 60;
                progreso?.Report($"Estado del batch: {status} - Tiempo: {minutos}m {segundos}s (m�x: 5min)");

                if (status == "completed")
                {
                    return json.RootElement.GetProperty("output_file_id").GetString() 
                        ?? throw new Exception("No se pudo obtener el output_file_id");
                }
                else if (status == "failed" || status == "expired" || status == "cancelled")
                {
                    // Registrar detalles del error
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.ERROR,
                        DsModulo = "OpenAIBL.EsperarResultadosAsync",
                        DsMensaje = $"Batch {batchId} fall�. Respuesta completa: {responseContent}"
                    });

                    throw new Exception($"El batch fall� con estado: {status}. Respuesta: {responseContent}");
                }

                // Esperar 5 segundos antes de volver a consultar
                await Task.Delay(5000, cancellationToken);
                intentos++;
            }

            // Si llegamos aqu�, el batch a�n est� proces�ndose pero se agot� el timeout
            // Guardar el batch_id en log para seguimiento manual
            logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.INFO,
                DsModulo = "OpenAIBL.EsperarResultadosAsync",
                DsMensaje = $"Timeout alcanzado. Batch {batchId} a�n en proceso. Estado: in_progress. " +
                           $"Puede consultar el estado manualmente en OpenAI o esperar notificaci�n."
            });

            throw new TimeoutException($"El batch {batchId} no complet� en 5 minutos. " +
                                     $"El proceso contin�a en OpenAI. Consulte el batch_id: {batchId}");
        }

        private static async Task ProcesarResultadosAsync(
            string resultFileId,
            List<ArchivoPagina> paginas,
            int cdLote,
            int cdProyecto,
            int cdUsuario)
        {
            var parametroDAL = new ParametrosDAL();
            string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY") 
                ?? throw new Exception("No se encontr� la API Key de OpenAI en par�metros");
            string? dsModelo = parametroDAL.ObtenerValor("OPENAI_MODEL");

            // Descargar archivo de resultados
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.openai.com/v1/files/{resultFileId}/content");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            var lines = resultContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var resultadoDAL = new ResultadoIADAL();
            var tokenDAL = new TokenDAL();
            var archivoPaginaDAL = new ArchivoPaginaDAL();

            foreach (var line in lines)
            {
                var json = JsonDocument.Parse(line);

                string customId = json.RootElement.GetProperty("custom_id").GetString() ?? "";
                int cdArchivoPagina = int.Parse(customId.Replace("page_", ""));

                var responseBody = json.RootElement.GetProperty("response").GetProperty("body");

                string respuestaIA = responseBody
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content").GetString() ?? "";

                var usage = responseBody.GetProperty("usage");
                int promptTokens = usage.GetProperty("prompt_tokens").GetInt32();
                int completionTokens = usage.GetProperty("completion_tokens").GetInt32();
                int totalTokens = usage.GetProperty("total_tokens").GetInt32();

                // Guardar resultado IA temporal
                // Por ahora guardamos el JSON crudo en el campo DsExpediente como placeholder
                int cdResultado = resultadoDAL.Insertar(new ResultadoIA
                {
                    CdLote = cdLote,
                    CdArchivoPagina = cdArchivoPagina,
                    DsExpediente = respuestaIA.Length > 200 ? respuestaIA.Substring(0, 200) : respuestaIA,
                    FeAlta = DateTime.Now,
                    CdUsuarioAlta = cdUsuario
                });

                // Guardar tokens
                tokenDAL.Insertar(new TokenConsumo
                {
                    CdResultado = cdResultado,
                    NuTokensPrompt = promptTokens,
                    NuTokensCompletion = completionTokens,
                    NuTokensTotal = totalTokens,
                    DsModelo = dsModelo,
                    FeAlta = DateTime.Now
                });

                // Actualizar estado de la p�gina a "Procesado IA (4)"
                archivoPaginaDAL.ActualizarEstado(cdArchivoPagina, 4, cdUsuario);
            }
        }

        /// <summary>
        /// Guarda el batch_id en la tabla de tracking para seguimiento
        /// </summary>
        private static void GuardarBatchTracking(int cdLote, string batchId, string fileId)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var comando = new SqlCommand(@"
                    INSERT INTO TD_BATCH_TRACKING 
                        (cdLote, dsBatchId, dsFileId, dsEstado, feAlta)
                    VALUES 
                        (@cdLote, @batchId, @fileId, 'created', GETDATE())", conexion);

                comando.Parameters.AddWithValue("@cdLote", cdLote);
                comando.Parameters.AddWithValue("@batchId", batchId);
                comando.Parameters.AddWithValue("@fileId", fileId);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Verifica el estado actual de un batch en OpenAI
        /// </summary>
        public static async Task<(string estado, string? outputFileId, string? errorFileId, int? totalRequests, int? completedRequests, int? failedRequests)> VerificarEstadoBatchAsync(string batchId)
        {
            var logDAL = new LogDAL();
            var parametroDAL = new ParametrosDAL();

            try
            {
                string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY") 
                    ?? throw new Exception("No se encontr� la API Key de OpenAI en par�metros");
                string? dsModelo = parametroDAL.ObtenerValor("OPENAI_MODEL");

                string url = $"https://api.openai.com/v1/batches/{batchId}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Log de la respuesta completa para debugging
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.VerificarEstadoBatchAsync",
                    DsMensaje = $"Respuesta API para verificar batch {batchId}: {responseBody}"
                });

                var batchInfo = JsonDocument.Parse(responseBody);

                string estado = batchInfo.RootElement.GetProperty("status").GetString() ?? "unknown";

                // Intentar obtener output_file_id (solo disponible cuando completed)
                string? outputFileId = null;
                if (batchInfo.RootElement.TryGetProperty("output_file_id", out var outputFileIdProp))
                {
                    outputFileId = outputFileIdProp.GetString();
                }

                // Intentar obtener error_file_id (solo disponible si hay errores)
                string? errorFileId = null;
                if (batchInfo.RootElement.TryGetProperty("error_file_id", out var errorFileIdProp))
                {
                    errorFileId = errorFileIdProp.GetString();
                }

                // Obtener contadores de requests
                int? totalRequests = null;
                int? completedRequests = null;
                int? failedRequests = null;

                if (batchInfo.RootElement.TryGetProperty("request_counts", out var requestCounts))
                {
                    if (requestCounts.TryGetProperty("total", out var total))
                        totalRequests = total.GetInt32();
                    if (requestCounts.TryGetProperty("completed", out var completed))
                        completedRequests = completed.GetInt32();
                    if (requestCounts.TryGetProperty("failed", out var failed))
                        failedRequests = failed.GetInt32();
                }

                // Log para seguimiento
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.VerificarEstadoBatchAsync",
                    DsMensaje = $"Batch {batchId} - Estado: {estado}, Output: {outputFileId ?? "N/A"}"
                });

                return (estado, outputFileId, errorFileId, totalRequests, completedRequests, failedRequests);
            }
            catch (Exception ex)
            {
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "OpenAIBL.VerificarEstadoBatchAsync",
                    DsMensaje = $"Error al verificar estado del batch {batchId}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });

                throw;
            }
        }

        /// <summary>
        /// Procesa los resultados de un batch completado
        /// </summary>
        public static async Task ProcesarResultadosBatchAsync(string batchId, int cdLote, int cdUsuario)
        {
            var logDAL = new LogDAL();
            var resultadoDAL = new ResultadoIADAL();
            var tokenDAL = new TokenDAL();
            var archivoPaginaDAL = new ArchivoPaginaDAL();
            var loteDAL = new LoteDAL();
            var parametroDAL = new ParametrosDAL();
            var categoriaPlanoDAL = new CategoriaPlanoDAL();
            var tipoPlanoDAL = new TipoPlanoDAL();
            var reparticionDAL = new ReparticionDAL();
            var reparticiones = reparticionDAL.ObtenerTodos();
            var resultadoIAErrorDAL = new ResultadoIAErrorDAL();
            int paginasConError = 0;

            try
            {
                string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY") 
                    ?? throw new Exception("No se encontr� la API Key de OpenAI en par�metros");
                string? dsModelo = parametroDAL.ObtenerValor("OPENAI_MODEL");

                // 1. Intentar obtener output_file_id de la BD primero
                string? outputFileId = null;
                using (var connection = new SqlConnection(Configuracion.CadenaConexion))
                {
                    connection.Open();
                    string query = "SELECT dsOutputFileId FROM TD_BATCH_TRACKING WHERE dsBatchId = @batchId";
                    using var cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@batchId", batchId);
                    var result = cmd.ExecuteScalar();
                    outputFileId = result?.ToString();
                }

                // Si no est� en BD, obtenerlo de la API
                if (string.IsNullOrEmpty(outputFileId))
                {
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.INFO,
                        DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                        DsMensaje = $"Output file ID no encontrado en BD, consultando API para batch {batchId}"
                    });

                    string url = $"https://api.openai.com/v1/batches/{batchId}";
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("Authorization", $"Bearer {apiKey}");

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Log completo de la respuesta para debugging
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.INFO,
                        DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                        DsMensaje = $"Respuesta completa de OpenAI para batch {batchId}: {responseBody}"
                    });

                    var batchInfo = JsonDocument.Parse(responseBody);

                    string estado = batchInfo.RootElement.GetProperty("status").GetString() ?? "";

                    if (estado != "completed")
                    {
                        throw new Exception($"El batch no est� completado. Estado actual: {estado}");
                    }

                    // Obtener errorFileId si existe
                    string? errorFileId = null;
                    if (batchInfo.RootElement.TryGetProperty("error_file_id", out var errorProp))
                    {
                        errorFileId = errorProp.GetString();
                    }

                    // Obtener contadores
                    int? totalRequests = null;
                    int? failedRequests = null;
                    if (batchInfo.RootElement.TryGetProperty("request_counts", out var requestCounts))
                    {
                        if (requestCounts.TryGetProperty("total", out var total))
                            totalRequests = total.GetInt32();
                        if (requestCounts.TryGetProperty("failed", out var failed))
                            failedRequests = failed.GetInt32();
                    }

                    if (batchInfo.RootElement.TryGetProperty("output_file_id", out var outputProp))
                    {
                        outputFileId = outputProp.GetString();

                        logDAL.Insertar(new LogRegistro
                        {
                            DsNivel = LogRegistro.Niveles.INFO,
                            DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                            DsMensaje = $"output_file_id obtenido de API: {outputFileId ?? "NULL"}"
                        });
                    }
                    else
                    {
                        logDAL.Insertar(new LogRegistro
                        {
                            DsNivel = LogRegistro.Niveles.WARNING,
                            DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                            DsMensaje = $"La respuesta de la API NO contiene la propiedad 'output_file_id' para batch {batchId}"
                        });
                    }

                    if (string.IsNullOrEmpty(outputFileId))
                    {
                        // Si no hay output pero hay error_file_id, descargar y mostrar los errores
                        if (!string.IsNullOrEmpty(errorFileId))
                        {
                            logDAL.Insertar(new LogRegistro
                            {
                                DsNivel = LogRegistro.Niveles.WARNING,
                                DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                                DsMensaje = $"Descargando archivo de errores para batch {batchId}: {errorFileId}"
                            });

                            string errorDownloadUrl = $"https://api.openai.com/v1/files/{errorFileId}/content";
                            var errorDownloadRequest = new HttpRequestMessage(HttpMethod.Get, errorDownloadUrl);
                            errorDownloadRequest.Headers.Add("Authorization", $"Bearer {apiKey}");

                            var errorDownloadResponse = await _httpClient.SendAsync(errorDownloadRequest);
                            errorDownloadResponse.EnsureSuccessStatusCode();

                            string errorContent = await errorDownloadResponse.Content.ReadAsStringAsync();

                            // Log del contenido de errores
                            logDAL.Insertar(new LogRegistro
                            {
                                DsNivel = LogRegistro.Niveles.ERROR,
                                DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                                DsMensaje = $"Errores del batch {batchId}:\n{errorContent}"
                            });

                            throw new Exception($"El batch no tiene resultados exitosos. Todas las requests fallaron ({failedRequests ?? 0} de {totalRequests ?? 0}). Revise los logs para ver los detalles de los errores.");
                        }

                        throw new Exception("El batch no tiene archivo de salida disponible. Verifique los logs para m�s detalles.");
                    }

                    // Actualizar BD con el output_file_id
                    using (var connection = new SqlConnection(Configuracion.CadenaConexion))
                    {
                        connection.Open();
                        string updateQuery = "UPDATE TD_BATCH_TRACKING SET dsOutputFileId = @outputFileId WHERE dsBatchId = @batchId";
                        using var updateCmd = new SqlCommand(updateQuery, connection);
                        updateCmd.Parameters.AddWithValue("@outputFileId", outputFileId);
                        updateCmd.Parameters.AddWithValue("@batchId", batchId);
                        updateCmd.ExecuteNonQuery();
                    }
                }

                // 2. Descargar archivo de resultados
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                    DsMensaje = $"Descargando resultados del batch {batchId}, archivo {outputFileId}"
                });

                string downloadUrl = $"https://api.openai.com/v1/files/{outputFileId}/content";
                var downloadRequest = new HttpRequestMessage(HttpMethod.Get, downloadUrl);
                downloadRequest.Headers.Add("Authorization", $"Bearer {apiKey}");

                var downloadResponse = await _httpClient.SendAsync(downloadRequest);
                downloadResponse.EnsureSuccessStatusCode();

                string resultContent = await downloadResponse.Content.ReadAsStringAsync();

                // LOG: Guardar archivo de salida completo para diagn�stico
                string outputFilePath = Path.Combine(Path.GetTempPath(), $"openai_batch_{batchId}_output.jsonl");
                await File.WriteAllTextAsync(outputFilePath, resultContent);

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                    DsMensaje = $"Archivo de salida guardado en: {outputFilePath}"
                });

                // 3. Procesar cada l�nea del archivo JSONL
                var lineas = resultContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                    DsMensaje = $"Procesando {lineas.Length} resultados del batch {batchId}"
                });

                foreach (string linea in lineas)
                {
                    var resultado = JsonDocument.Parse(linea);

                    // Obtener custom_id que contiene el cdArchivoPagina: "page_1234" -> 1234
                    string customId = resultado.RootElement.GetProperty("custom_id").GetString() ?? "";

                    // Extraer solo el n�mero del custom_id (formato: "page_XXXX")
                    string numeroStr = customId.Replace("page_", "").Replace("request-", "");
                    int cdArchivoPagina = int.Parse(numeroStr);

                    // Verificar si hay error REAL (no null)
                    if (resultado.RootElement.TryGetProperty("error", out var errorElement) && 
                        errorElement.ValueKind != JsonValueKind.Null)
                    {
                        string errorMessage = errorElement.ToString();
                        logDAL.Insertar(new LogRegistro
                        {
                            DsNivel = LogRegistro.Niveles.WARNING,
                            DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                            DsMensaje = $"Error en p�gina {cdArchivoPagina} del batch {batchId}: {errorMessage}"
                        });

                        archivoPaginaDAL.ActualizarEstado(cdArchivoPagina, 5, cdUsuario);
                        resultadoIAErrorDAL.Insertar(new ResultadoIAError
                        {
                            CdLote = cdLote,
                            CdArchivoPagina = cdArchivoPagina,
                            DsMotivoError = $"Error devuelto por OpenAI: {errorMessage}",
                            DsRespuestaCruda = null
                        });
                        paginasConError++;
                        continue;
                    }

                    // Obtener respuesta
                    var responseObj = resultado.RootElement.GetProperty("response");
                    var body = responseObj.GetProperty("body");
                    var choices = body.GetProperty("choices");
                    var firstChoice = choices[0];
                    var message = firstChoice.GetProperty("message");
                    string respuestaIA = message.GetProperty("content").GetString() ?? "";

                    // Verificar si la respuesta fue truncada por l�mite de tokens
                    if (firstChoice.TryGetProperty("finish_reason", out var finishReasonElement) &&
                        finishReasonElement.GetString() == "length")
                    {
                        logDAL.Insertar(new LogRegistro
                        {
                            DsNivel = LogRegistro.Niveles.WARNING,
                            DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                            DsMensaje = $"La respuesta de la p�gina {cdArchivoPagina} fue truncada por l�mite de max_tokens (finish_reason=length). El JSON puede estar incompleto."
                        });
                    }

                    // LOG: Ver respuesta RAW antes de limpiar
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.INFO,
                        DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                        DsMensaje = $"P�gina {cdArchivoPagina} - Respuesta RAW (primeros 500 chars): {respuestaIA.Substring(0, Math.Min(500, respuestaIA.Length))}"
                    });

                    // Limpiar markdown code fences si est�n presentes
                    respuestaIA = respuestaIA.Trim();
                    if (respuestaIA.StartsWith("```json"))
                    {
                        respuestaIA = respuestaIA.Substring(7).Trim(); // Quitar ```json
                    }
                    else if (respuestaIA.StartsWith("```"))
                    {
                        respuestaIA = respuestaIA.Substring(3).Trim(); // Quitar ```
                    }

                    if (respuestaIA.EndsWith("```"))
                    {
                        respuestaIA = respuestaIA.Substring(0, respuestaIA.Length - 3).Trim(); // Quitar ```
                    }

                    // Parsear JSON de OpenAI
                    RespuestaOpenAI? datosExtraidos = null;
                    try
                    {
                        datosExtraidos = JsonSerializer.Deserialize<RespuestaOpenAI>(respuestaIA);
                    }
                    catch (JsonException ex)
                    {
                        logDAL.Insertar(new LogRegistro
                        {
                            DsNivel = LogRegistro.Niveles.ERROR,
                            DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                            DsMensaje = $"Error al parsear JSON de p�gina {cdArchivoPagina}: {ex.Message}. JSON: {respuestaIA.Substring(0, Math.Min(500, respuestaIA.Length))}"
                        });

                        archivoPaginaDAL.ActualizarEstado(cdArchivoPagina, 5, cdUsuario);
                        resultadoIAErrorDAL.Insertar(new ResultadoIAError
                        {
                            CdLote = cdLote,
                            CdArchivoPagina = cdArchivoPagina,
                            DsMotivoError = $"Error al parsear JSON: {ex.Message}",
                            DsRespuestaCruda = respuestaIA.Substring(0, Math.Min(4000, respuestaIA.Length))
                        });
                        paginasConError++;
                        continue;
                    }

                    if (datosExtraidos == null)
                    {
                        logDAL.Insertar(new LogRegistro
                        {
                            DsNivel = LogRegistro.Niveles.WARNING,
                            DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                            DsMensaje = $"JSON deserializado es null para p�gina {cdArchivoPagina}"
                        });

                        archivoPaginaDAL.ActualizarEstado(cdArchivoPagina, 5, cdUsuario);
                        resultadoIAErrorDAL.Insertar(new ResultadoIAError
                        {
                            CdLote = cdLote,
                            CdArchivoPagina = cdArchivoPagina,
                            DsMotivoError = "JSON deserializado resultó en null",
                            DsRespuestaCruda = respuestaIA.Substring(0, Math.Min(4000, respuestaIA.Length))
                        });
                        paginasConError++;
                        continue;
                    }

                    // Resolver cdCategoriaPlano y cdTipoPlano contra los catalogos, comparando de forma normalizada
                    int? cdCategoriaPlano = null;
                    int? cdTipoPlano = null;

                    if (!string.IsNullOrWhiteSpace(datosExtraidos.categoriaPlano))
                    {
                        string categoriaNormalizada = NormalizarTexto(datosExtraidos.categoriaPlano);
                        var categoria = categoriaPlanoDAL.ObtenerTodos()
                            .FirstOrDefault(c => NormalizarTexto(c.DsCategoriaPlano) == categoriaNormalizada);
                        cdCategoriaPlano = categoria?.CdCategoriaPlano;
                    }

                    if (!string.IsNullOrWhiteSpace(datosExtraidos.tipoPlano))
                    {
                        string tipoNormalizado = NormalizarTexto(datosExtraidos.tipoPlano);
                        var tipo = tipoPlanoDAL.ObtenerTodos()
                            .Where(t => !cdCategoriaPlano.HasValue || t.CdCategoriaPlano == cdCategoriaPlano.Value)
                            .FirstOrDefault(t => NormalizarTexto(t.DsTipoPlano) == tipoNormalizado);
                        cdTipoPlano = tipo?.CdTipoPlano;
                    }

                    // Obtener tokens
                    var usage = body.GetProperty("usage");
                    int promptTokens = usage.GetProperty("prompt_tokens").GetInt32();
                    int completionTokens = usage.GetProperty("completion_tokens").GetInt32();
                    int totalTokens = usage.GetProperty("total_tokens").GetInt32();

                    // Normalizar campos extraidos antes de persistir
                    string? dsExpedienteNormalizado = NormalizarExpediente(
                        LimpiarTexto(datosExtraidos.expediente), reparticiones, out bool expedienteValido);
                    decimal? nuConfianzaExpediente = expedienteValido
                        ? datosExtraidos.confianza?.expediente
                        : 0.5000m;

                    string? dsSeccionNormalizada = NormalizarSeccion(LimpiarTexto(datosExtraidos.seccion));
                    string? dsManzanaNormalizada = NormalizarManzana(LimpiarTexto(datosExtraidos.manzana));
                    string? dsParcelaNormalizada = NormalizarParcela(LimpiarTexto(datosExtraidos.parcela));
                    string? dsDireccionNormalizada = NormalizarDireccion(LimpiarTexto(datosExtraidos.direccion));

                    // Guardar resultado con campos parseados
                    int cdResultado = resultadoDAL.Insertar(new ResultadoIA
                    {
                        CdLote = cdLote,
                        CdArchivoPagina = cdArchivoPagina,
                        CdCategoriaPlano = cdCategoriaPlano,
                        CdTipoPlano = cdTipoPlano,
                        DsExpediente = dsExpedienteNormalizado,
                        DsSeccion = dsSeccionNormalizada,
                        DsManzana = dsManzanaNormalizada,
                        DsParcela = dsParcelaNormalizada,
                        DsDireccion = dsDireccionNormalizada,
                        DsNumeroPlano = LimpiarTexto(datosExtraidos.numeroPlano),
                        NuConfianzaCategoriaPlano = datosExtraidos.confianza?.categoriaPlano,
                        NuConfianzaTipoPlano = datosExtraidos.confianza?.tipoPlano,
                        NuConfianzaExpediente = nuConfianzaExpediente,
                        NuConfianzaSeccion = datosExtraidos.confianza?.seccion,
                        NuConfianzaManzana = datosExtraidos.confianza?.manzana,
                        NuConfianzaParcela = datosExtraidos.confianza?.parcela,
                        NuConfianzaDireccion = datosExtraidos.confianza?.direccion,
                        NuConfianzaNumeroPlano = datosExtraidos.confianza?.numeroPlano,
                        FeAlta = DateTime.Now,
                        CdUsuarioAlta = cdUsuario
                    });

                    // Guardar tokens
                    tokenDAL.Insertar(new TokenConsumo
                    {
                        CdResultado = cdResultado,
                        NuTokensPrompt = promptTokens,
                        NuTokensCompletion = completionTokens,
                        NuTokensTotal = totalTokens,
                        DsModelo = dsModelo,
                        FeAlta = DateTime.Now
                    });

                    // Actualizar estado de la p�gina
                    archivoPaginaDAL.ActualizarEstado(cdArchivoPagina, 4, cdUsuario);
                }

                // 4. Actualizar estado del lote: solo se marca "Procesado (4)" si no hubo
                //    paginas con error de procesamiento/parseo. Si hubo errores, se deja el
                //    lote en estado "Listo para IA (2)" para que pueda reprocesarse.
                if (paginasConError == 0)
                {
                    loteDAL.ActualizarEstado(cdLote, 4, cdUsuario);
                }
                else
                {
                    loteDAL.ActualizarEstado(cdLote, 2, cdUsuario);
                }

                // 5. Marcar batch como resultado procesado
                using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
                {
                    string updateQuery = "UPDATE TD_BATCH_TRACKING SET snResultadoProcesado = 'SI' WHERE dsBatchId = @batchId";
                    var comando = new SqlCommand(updateQuery, conexion);
                    comando.Parameters.AddWithValue("@batchId", batchId);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }

                if (paginasConError == 0)
                {
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.INFO,
                        DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                        DsMensaje = $"Batch {batchId} procesado exitosamente. Lote {cdLote} completado."
                    });
                }
                else
                {
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.WARNING,
                        DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                        DsMensaje = $"Batch {batchId} procesado con {paginasConError} página(s) en error. Lote {cdLote} quedó en estado 'Listo para IA' para reprocesar."
                    });
                }
            }
            catch (Exception ex)
            {
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                    DsMensaje = $"Error al procesar resultados del batch {batchId}: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });

                throw;
            }
        }
    }
}
