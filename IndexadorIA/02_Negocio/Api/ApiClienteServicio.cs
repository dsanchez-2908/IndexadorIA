using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IndexadorIA.Negocio.Api
{
    /// <summary>
    /// Cliente HTTP para consumir IndexadorIA.Api desde la aplicación WinForms.
    /// Lee la URL base y la API Key desde App.config (claves "UrlApi" y "ApiKeyApi").
    /// </summary>
    public class ApiClienteServicio
    {
        private readonly HttpClient _httpClient;

        public ApiClienteServicio()
        {
            string urlApi = ConfigurationManager.AppSettings["UrlApi"] ?? "http://localhost:5090/";
            string apiKey = ConfigurationManager.AppSettings["ApiKeyApi"] ?? string.Empty;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(urlApi)
            };
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
        }

        /// <summary>
        /// Aplica el token JWT actual (si existe) a las próximas solicitudes.
        /// </summary>
        private void AplicarToken()
        {
            if (!string.IsNullOrEmpty(SesionApi.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SesionApi.Token);
            }
        }

        /// <summary>
        /// Llama a POST /api/auth/login. Lanza ApiException con el mensaje de error si falla.
        /// </summary>
        public async Task<LoginApiResponseDto> LoginAsync(string dsUsuario, string dsClave)
        {
            var request = new LoginApiRequestDto { DsUsuario = dsUsuario, DsClave = dsClave };

            HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync("api/auth/login", request).ConfigureAwait(false);

            if (!respuesta.IsSuccessStatusCode)
            {
                string mensaje = await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false);
                throw new ApiException(mensaje);
            }

            var resultado = await respuesta.Content.ReadFromJsonAsync<LoginApiResponseDto>().ConfigureAwait(false);
            return resultado ?? throw new ApiException("Respuesta vacía del servidor.");
        }

        /// <summary>
        /// Llama a POST /api/auth/cambiar-clave-temporal. Lanza ApiException con el mensaje de error si falla.
        /// </summary>
        public async Task<LoginApiResponseDto> CambiarClaveTemporalAsync(
            string dsUsuario, string claveTemporal, string nuevaClave, string confirmarClave)
        {
            var request = new CambiarClaveTemporalApiRequestDto
            {
                DsUsuario = dsUsuario,
                ClaveTemporal = claveTemporal,
                NuevaClave = nuevaClave,
                ConfirmarClave = confirmarClave
            };

            HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync("api/auth/cambiar-clave-temporal", request).ConfigureAwait(false);

            if (!respuesta.IsSuccessStatusCode)
            {
                string mensaje = await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false);
                throw new ApiException(mensaje);
            }

            var resultado = await respuesta.Content.ReadFromJsonAsync<LoginApiResponseDto>().ConfigureAwait(false);
            return resultado ?? throw new ApiException("Respuesta vacía del servidor.");
        }

        /// <summary>
        /// Llama a POST /api/auth/cambiar-clave (usuario ya autenticado, sesión normal).
        /// Lanza ApiException con el mensaje de error si falla.
        /// </summary>
        public async Task CambiarClaveAsync(string claveActual, string nuevaClave, string confirmarClave)
        {
            AplicarToken();

            var request = new CambiarClaveApiRequestDto
            {
                ClaveActual = claveActual,
                NuevaClave = nuevaClave,
                ConfirmarClave = confirmarClave
            };

            HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync("api/auth/cambiar-clave", request).ConfigureAwait(false);

            if (!respuesta.IsSuccessStatusCode)
            {
                string mensaje = await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false);
                throw new ApiException(mensaje);
            }
        }

        /// <summary>
        /// Llama a GET /api/lotes con los filtros indicados (equivalente remoto de FrmControlFinalizacion.CargarLotes()).
        /// </summary>
        public async Task<List<LoteResumenApiDto>> ObtenerLotesEnControlAsync(
            string? dsNombreLote, DateTime? feAltaDesde, DateTime? feAltaHasta)
        {
            AplicarToken();

            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(dsNombreLote))
                query.Add($"dsNombreLote={Uri.EscapeDataString(dsNombreLote)}");
            if (feAltaDesde.HasValue)
                query.Add($"feAltaDesde={Uri.EscapeDataString(feAltaDesde.Value.ToString("o"))}");
            if (feAltaHasta.HasValue)
                query.Add($"feAltaHasta={Uri.EscapeDataString(feAltaHasta.Value.ToString("o"))}");

            string url = "api/lotes" + (query.Count > 0 ? "?" + string.Join("&", query) : string.Empty);

            HttpResponseMessage respuesta = await _httpClient.GetAsync(url).ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));

            var resultado = await respuesta.Content.ReadFromJsonAsync<List<LoteResumenApiDto>>().ConfigureAwait(false);
            return resultado ?? new List<LoteResumenApiDto>();
        }

        /// <summary>
        /// Llama a GET /api/lotes/{cdLote}/detalle (equivalente remoto de FrmVerLote.CargarEncabezadoLote() + CargarDatosGrilla()).
        /// </summary>
        public async Task<LoteDetalleApiDto> ObtenerDetalleLoteAsync(int cdLote, bool mostrarTodos = true)
        {
            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.GetAsync($"api/lotes/{cdLote}/detalle?mostrarTodos={mostrarTodos}").ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));

            var resultado = await respuesta.Content.ReadFromJsonAsync<LoteDetalleApiDto>().ConfigureAwait(false);
            return resultado ?? throw new ApiException("Respuesta vacía del servidor.");
        }

        /// <summary>
        /// Llama a PUT /api/resultados/{cdResultado}/datos (equivalente remoto de la corrección de datos en FrmVerLote).
        /// </summary>
        public async Task ActualizarDatosResultadoAsync(int cdResultado, ActualizarResultadoApiRequestDto request)
        {
            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.PutAsJsonAsync($"api/resultados/{cdResultado}/datos", request).ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));
        }

        /// <summary>
        /// Llama a PUT /api/resultados/{cdResultado}/estado-control (equivalente remoto de FrmVerLote.MarcarEstadoControl(...)).
        /// </summary>
        public async Task ActualizarEstadoControlResultadoAsync(int cdResultado, ActualizarEstadoControlApiRequestDto request)
        {
            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.PutAsJsonAsync($"api/resultados/{cdResultado}/estado-control", request).ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));
        }

        /// <summary>
        /// Llama a POST /api/resultados/{cdResultado}/correcciones (equivalente remoto de
        /// FrmVerLote.RegistrarCorreccionesSiCorresponde(...)).
        /// </summary>
        public async Task RegistrarCorreccionesResultadoAsync(int cdResultado, RegistrarCorreccionesApiRequestDto request)
        {
            if (request.Correcciones.Count == 0)
                return;

            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync($"api/resultados/{cdResultado}/correcciones", request).ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));
        }

        /// <summary>
        /// Llama a GET /api/archivos/lotes/{cdLote}/paginas/{cdArchivoPagina}/pdf y devuelve los bytes del PDF.
        /// </summary>
        public async Task<byte[]> ObtenerPdfAsync(int cdLote, int cdArchivoPagina)
        {
            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.GetAsync($"api/archivos/lotes/{cdLote}/paginas/{cdArchivoPagina}/pdf").ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));

            return await respuesta.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Llama a GET /api/archivos/lotes/{cdLote}/paginas/{cdArchivoPagina}/jpg y devuelve los bytes del JPG.
        /// </summary>
        public async Task<byte[]> ObtenerJpgAsync(int cdLote, int cdArchivoPagina)
        {
            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.GetAsync($"api/archivos/lotes/{cdLote}/paginas/{cdArchivoPagina}/jpg").ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));

            return await respuesta.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Llama a PUT /api/lotes/{cdLote}/pendiente-finalizar (equivalente remoto de
        /// FrmVerLote.btnMarcarLoteCompletado_Click, solo el cambio de estado del lote).
        /// </summary>
        public async Task MarcarLotePendienteFinalizarAsync(int cdLote)
        {
            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.PutAsync($"api/lotes/{cdLote}/pendiente-finalizar", null).ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));
        }

        /// <summary>
        /// Llama a GET /api/combos/categorias-plano.
        /// </summary>
        public async Task<List<CategoriaPlanoApiDto>> ObtenerCategoriasPlanoAsync()
        {
            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.GetAsync("api/combos/categorias-plano").ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));

            var resultado = await respuesta.Content.ReadFromJsonAsync<List<CategoriaPlanoApiDto>>().ConfigureAwait(false);
            return resultado ?? new List<CategoriaPlanoApiDto>();
        }

        /// <summary>
        /// Llama a GET /api/combos/tipos-plano.
        /// </summary>
        public async Task<List<TipoPlanoApiDto>> ObtenerTiposPlanoAsync()
        {
            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.GetAsync("api/combos/tipos-plano").ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));

            var resultado = await respuesta.Content.ReadFromJsonAsync<List<TipoPlanoApiDto>>().ConfigureAwait(false);
            return resultado ?? new List<TipoPlanoApiDto>();
        }

        /// <summary>
        /// Llama a GET /api/combos/reparticiones.
        /// </summary>
        public async Task<List<ReparticionApiDto>> ObtenerReparticionesAsync()
        {
            AplicarToken();

            HttpResponseMessage respuesta = await _httpClient.GetAsync("api/combos/reparticiones").ConfigureAwait(false);
            if (!respuesta.IsSuccessStatusCode)
                throw new ApiException(await ObtenerMensajeErrorAsync(respuesta).ConfigureAwait(false));

            var resultado = await respuesta.Content.ReadFromJsonAsync<List<ReparticionApiDto>>().ConfigureAwait(false);
            return resultado ?? new List<ReparticionApiDto>();
        }

        private static async Task<string> ObtenerMensajeErrorAsync(HttpResponseMessage respuesta)
        {
            try
            {
                var error = await respuesta.Content.ReadFromJsonAsync<ApiMensajeDto>().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(error?.Mensaje))
                    return error!.Mensaje!;
            }
            catch
            {
                // Ignorado: se usa el mensaje por defecto basado en el código de estado.
            }

            return respuesta.StatusCode switch
            {
                System.Net.HttpStatusCode.Unauthorized => "Usuario o contraseña incorrectos.",
                System.Net.HttpStatusCode.Forbidden => "El usuario no tiene permitido el ingreso remoto.",
                _ => $"Error de comunicación con la API ({(int)respuesta.StatusCode})."
            };
        }
    }

    public class ApiException : Exception
    {
        public ApiException(string mensaje) : base(mensaje) { }
    }
}
