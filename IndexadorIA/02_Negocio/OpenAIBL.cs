using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// Lógica de negocio para procesamiento de lotes con OpenAI Batch API
    /// </summary>
    public static class OpenAIBL
    {
        private static readonly HttpClient _httpClient = new HttpClient();

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
                    throw new Exception("No se encontró un prompt configurado para el proyecto");
                }

                // 2. Obtener las páginas del lote
                progreso?.Report("Obteniendo páginas del lote");
                var paginas = loteDAL.ObtenerArchivosPaginasPorLote(cdLote);

                if (paginas.Count == 0)
                {
                    throw new Exception("No se encontraron páginas para procesar en el lote");
                }

                // 3. Crear archivo JSONL para batch
                progreso?.Report($"Creando archivo batch con {paginas.Count} página(s)");
                string batchFilePath = await CrearArchivoBatchAsync(paginas, prompt.DsPrompt, cdProyecto);

                // 4. Subir archivo a OpenAI
                progreso?.Report("Subiendo archivo a OpenAI");
                string fileId = await SubirArchivoAsync(batchFilePath);

                // 5. Crear batch job
                progreso?.Report("Creando batch job en OpenAI");
                string batchId = await CrearBatchJobAsync(fileId);

                // 5.1 Guardar batch tracking en BD
                GuardarBatchTracking(cdLote, batchId, fileId);

                // 6. Esperar resultados
                progreso?.Report($"Esperando resultados del batch {batchId} (esto puede tardar varios minutos)");
                string resultFileId = await EsperarResultadosAsync(batchId, progreso, cancellationToken);

                // 7. Procesar resultados
                progreso?.Report("Descargando y procesando resultados");
                await ProcesarResultadosAsync(resultFileId, paginas, cdLote, cdProyecto, cdUsuario);

                // 8. Cambiar estado del lote a "Procesado IA (4)"
                loteDAL.ActualizarEstado(cdLote, 4, cdUsuario);

                // Limpiar archivo temporal
                if (File.Exists(batchFilePath))
                {
                    File.Delete(batchFilePath);
                }

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
                // El batch fue enviado exitosamente pero aún está procesándose
                // NO revertir el estado del lote - dejarlo en estado 3 (Procesando)

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarLoteAsync",
                    DsMensaje = $"Lote {cdLote} enviado a OpenAI. Procesamiento en segundo plano. {timeoutEx.Message}"
                });

                progreso?.Report($"Lote enviado a OpenAI - Procesando en segundo plano");
                return true; // Retornar true porque el batch SÍ fue creado exitosamente
            }
            catch (HttpRequestException httpEx)
            {
                // Error de red/conexión DESPUÉS de que el batch fue creado
                // Si llegamos aquí después de GuardarBatchTracking, el batch SÍ existe en OpenAI
                // NO revertir el estado del lote - dejarlo en estado 3 (Procesando)

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.WARNING,
                    DsModulo = "OpenAIBL.ProcesarLoteAsync",
                    DsMensaje = $"Lote {cdLote} enviado a OpenAI pero ocurrió error de red al consultar estado. El batch continúa procesándose en segundo plano. Error: {httpEx.Message}"
                });

                progreso?.Report($"Lote enviado a OpenAI - Error de red al consultar estado, pero el batch sigue procesándose");
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

        private static async Task<string> CrearArchivoBatchAsync(List<ArchivoPagina> paginas, string promptTemplate, int cdProyecto)
        {
            var jsonlLines = new List<string>();
            var logDAL = new LogDAL();

            foreach (var pagina in paginas)
            {
                // Log de diagnóstico: ruta del archivo Base64
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                    DsMensaje = $"Página {pagina.CdArchivoPagina}: Ruta Base64 = {pagina.RutaBase64}"
                });

                // Leer Base64 desde archivo
                if (string.IsNullOrEmpty(pagina.RutaBase64) || !File.Exists(pagina.RutaBase64))
                {
                    throw new Exception($"No se encontró el archivo Base64 para la página {pagina.CdArchivoPagina}");
                }

                string base64Content = await File.ReadAllTextAsync(pagina.RutaBase64);

                // Log del tamaño del Base64
                long base64SizeBytes = base64Content.Length;
                long base64SizeMB = base64SizeBytes / (1024 * 1024);

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                    DsMensaje = $"Página {pagina.CdArchivoPagina}: Tamaño Base64 = {base64SizeBytes:N0} bytes ({base64SizeMB} MB)"
                });

                // Validar tamaño (OpenAI limita a 20MB por imagen)
                const long MAX_SIZE_BYTES = 20 * 1024 * 1024; // 20 MB
                string imageDetail = "auto"; // Por defecto "auto"

                if (base64SizeBytes > MAX_SIZE_BYTES)
                {
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.WARNING,
                        DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                        DsMensaje = $"ADVERTENCIA: Página {pagina.CdArchivoPagina} excede 20MB ({base64SizeMB} MB). Esto puede causar errores 500 en OpenAI."
                    });
                    imageDetail = "low"; // Forzar detalle bajo para imágenes grandes
                }
                else if (base64SizeBytes > 5 * 1024 * 1024) // Mayor a 5MB
                {
                    // Para imágenes entre 5MB y 20MB, usar detalle bajo por seguridad
                    imageDetail = "low";
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.INFO,
                        DsModulo = "OpenAIBL.CrearArchivoBatchAsync",
                        DsMensaje = $"Página {pagina.CdArchivoPagina}: Usando detail='low' debido al tamaño ({base64SizeMB} MB)"
                    });
                }

                // Usar el prompt tal cual (sin reemplazos por ahora, se pueden agregar después)
                string promptFinal = promptTemplate;

                // Crear objeto de request según formato Batch API
                var batchItem = new
                {
                    custom_id = $"page_{pagina.CdArchivoPagina}",
                    method = "POST",
                    url = "/v1/chat/completions",
                    body = new
                    {
                        model = "gpt-4o-2024-08-06",  // Modelo con fecha específica (requerido para Batch API)
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
                        max_tokens = 2000
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
            // Escribir cada línea con \n (Unix line ending) en lugar de \r\n (Windows)
            await File.WriteAllTextAsync(tempPath, string.Join("\n", jsonlLines) + "\n", utf8WithoutBom);

            // Log del contenido para diagnóstico (solo primera línea)
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
                    DsMensaje = $"Archivo JSONL creado con {jsonlLines.Count} líneas. Primera línea: {primeraLinea}"
                });
            }

            return tempPath;
        }

        private static async Task<string> SubirArchivoAsync(string filePath)
        {
            var parametroDAL = new ParametrosDAL();
            string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY") 
                ?? throw new Exception("No se encontró la API Key de OpenAI en parámetros");

            // Log de diagnóstico PRE-subida
            var logDAL = new LogDAL();
            var fileInfo = new FileInfo(filePath);
            string fileName = Path.GetFileName(filePath);

            logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.INFO,
                DsModulo = "OpenAIBL.SubirArchivoAsync",
                DsMensaje = $"Preparando subida - Archivo: {fileName}, Tamaño: {fileInfo.Length} bytes, Existe: {fileInfo.Exists}"
            });

            // Leer contenido para validar
            string[] lines = await File.ReadAllLinesAsync(filePath);
            logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.INFO,
                DsModulo = "OpenAIBL.SubirArchivoAsync",
                DsMensaje = $"Contenido JSONL - Líneas: {lines.Length}, Primera línea (primeros 200 chars): {(lines.Length > 0 ? lines[0].Substring(0, Math.Min(200, lines[0].Length)) : "VACÍO")}"
            });

            using var formContent = new MultipartFormDataContent();

            var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
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

        private static async Task<string> CrearBatchJobAsync(string fileId)
        {
            var parametroDAL = new ParametrosDAL();
            string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY") 
                ?? throw new Exception("No se encontró la API Key de OpenAI en parámetros");

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

            // Log de diagnóstico
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
                ?? throw new Exception("No se encontró la API Key de OpenAI en parámetros");

            var logDAL = new LogDAL();
            int intentos = 0;
            const int maxIntentos = 60; // 5 minutos (5 seg * 60) - para pruebas

            while (intentos < maxIntentos)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.openai.com/v1/batches/{batchId}");
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

                var response = await _httpClient.SendAsync(request, cancellationToken);

                // Registrar toda la respuesta para diagnóstico
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                // Log para diagnóstico - solo en los primeros 3 intentos
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
                progreso?.Report($"Estado del batch: {status} - Tiempo: {minutos}m {segundos}s (máx: 5min)");

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
                        DsMensaje = $"Batch {batchId} falló. Respuesta completa: {responseContent}"
                    });

                    throw new Exception($"El batch falló con estado: {status}");
                }

                // Esperar 5 segundos antes de volver a consultar
                await Task.Delay(5000, cancellationToken);
                intentos++;
            }

            // Si llegamos aquí, el batch aún está procesándose pero se agotó el timeout
            // Guardar el batch_id en log para seguimiento manual
            logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.INFO,
                DsModulo = "OpenAIBL.EsperarResultadosAsync",
                DsMensaje = $"Timeout alcanzado. Batch {batchId} aún en proceso. Estado: in_progress. " +
                           $"Puede consultar el estado manualmente en OpenAI o esperar notificación."
            });

            throw new TimeoutException($"El batch {batchId} no completó en 5 minutos. " +
                                     $"El proceso continúa en OpenAI. Consulte el batch_id: {batchId}");
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
                ?? throw new Exception("No se encontró la API Key de OpenAI en parámetros");

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
                    FeAlta = DateTime.Now
                });

                // Actualizar estado de la página a "Procesado IA (4)"
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
                    ?? throw new Exception("No se encontró la API Key de OpenAI en parámetros");

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

            try
            {
                string apiKey = parametroDAL.ObtenerValor("OPENAI_API_KEY") 
                    ?? throw new Exception("No se encontró la API Key de OpenAI en parámetros");

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

                // Si no está en BD, obtenerlo de la API
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
                        throw new Exception($"El batch no está completado. Estado actual: {estado}");
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

                        throw new Exception("El batch no tiene archivo de salida disponible. Verifique los logs para más detalles.");
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

                // LOG: Guardar archivo de salida completo para diagnóstico
                string outputFilePath = Path.Combine(Path.GetTempPath(), $"openai_batch_{batchId}_output.jsonl");
                await File.WriteAllTextAsync(outputFilePath, resultContent);

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                    DsMensaje = $"Archivo de salida guardado en: {outputFilePath}"
                });

                // 3. Procesar cada línea del archivo JSONL
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

                    // Extraer solo el número del custom_id (formato: "page_XXXX")
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
                            DsMensaje = $"Error en página {cdArchivoPagina} del batch {batchId}: {errorMessage}"
                        });
                        continue;
                    }

                    // Obtener respuesta
                    var responseObj = resultado.RootElement.GetProperty("response");
                    var body = responseObj.GetProperty("body");
                    var choices = body.GetProperty("choices");
                    var firstChoice = choices[0];
                    var message = firstChoice.GetProperty("message");
                    string respuestaIA = message.GetProperty("content").GetString() ?? "";

                    // Verificar si la respuesta fue truncada por límite de tokens
                    if (firstChoice.TryGetProperty("finish_reason", out var finishReasonElement) &&
                        finishReasonElement.GetString() == "length")
                    {
                        logDAL.Insertar(new LogRegistro
                        {
                            DsNivel = LogRegistro.Niveles.WARNING,
                            DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                            DsMensaje = $"La respuesta de la página {cdArchivoPagina} fue truncada por límite de max_tokens (finish_reason=length). El JSON puede estar incompleto."
                        });
                    }

                    // LOG: Ver respuesta RAW antes de limpiar
                    logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.INFO,
                        DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                        DsMensaje = $"Página {cdArchivoPagina} - Respuesta RAW (primeros 500 chars): {respuestaIA.Substring(0, Math.Min(500, respuestaIA.Length))}"
                    });

                    // Limpiar markdown code fences si están presentes
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
                            DsMensaje = $"Error al parsear JSON de página {cdArchivoPagina}: {ex.Message}. JSON: {respuestaIA.Substring(0, Math.Min(500, respuestaIA.Length))}"
                        });
                        continue;
                    }

                    if (datosExtraidos == null)
                    {
                        logDAL.Insertar(new LogRegistro
                        {
                            DsNivel = LogRegistro.Niveles.WARNING,
                            DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                            DsMensaje = $"JSON deserializado es null para página {cdArchivoPagina}"
                        });
                        continue;
                    }

                    // Mapear cdTipoPlano según el valor de tipoPlano
                    int? cdTipoPlano = null;
                    if (!string.IsNullOrEmpty(datosExtraidos.tipoPlano))
                    {
                        cdTipoPlano = datosExtraidos.tipoPlano.ToUpper() switch
                        {
                            "OBRA" => 1,
                            "MENSURA" => 2,
                            "INSTALACIONES" => 3,
                            _ => null
                        };
                    }

                    // Obtener tokens
                    var usage = body.GetProperty("usage");
                    int promptTokens = usage.GetProperty("prompt_tokens").GetInt32();
                    int completionTokens = usage.GetProperty("completion_tokens").GetInt32();
                    int totalTokens = usage.GetProperty("total_tokens").GetInt32();

                    // Guardar resultado con campos parseados
                    int cdResultado = resultadoDAL.Insertar(new ResultadoIA
                    {
                        CdLote = cdLote,
                        CdArchivoPagina = cdArchivoPagina,
                        CdTipoPlano = cdTipoPlano,
                        DsExpediente = datosExtraidos.expediente,
                        DsSeccion = datosExtraidos.seccion,
                        DsManzana = datosExtraidos.manzana,
                        DsParcela = datosExtraidos.parcela,
                        DsDireccion = datosExtraidos.direccion,
                        NuConfianzaTipoPlano = datosExtraidos.confianza?.tipoPlano,
                        NuConfianzaExpediente = datosExtraidos.confianza?.expediente,
                        NuConfianzaSeccion = datosExtraidos.confianza?.seccion,
                        NuConfianzaManzana = datosExtraidos.confianza?.manzana,
                        NuConfianzaParcela = datosExtraidos.confianza?.parcela,
                        NuConfianzaDireccion = datosExtraidos.confianza?.direccion,
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
                        FeAlta = DateTime.Now
                    });

                    // Actualizar estado de la página
                    archivoPaginaDAL.ActualizarEstado(cdArchivoPagina, 4, cdUsuario);
                }

                // 4. Actualizar estado del lote a "Procesado (4)"
                loteDAL.ActualizarEstado(cdLote, 4, cdUsuario);

                // 5. Marcar batch como resultado procesado
                using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
                {
                    string updateQuery = "UPDATE TD_BATCH_TRACKING SET snResultadoProcesado = 'SI' WHERE dsBatchId = @batchId";
                    var comando = new SqlCommand(updateQuery, conexion);
                    comando.Parameters.AddWithValue("@batchId", batchId);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }

                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "OpenAIBL.ProcesarResultadosBatchAsync",
                    DsMensaje = $"Batch {batchId} procesado exitosamente. Lote {cdLote} completado."
                });
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
