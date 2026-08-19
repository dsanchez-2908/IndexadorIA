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

            HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (!respuesta.IsSuccessStatusCode)
            {
                string mensaje = await ObtenerMensajeErrorAsync(respuesta);
                throw new ApiException(mensaje);
            }

            var resultado = await respuesta.Content.ReadFromJsonAsync<LoginApiResponseDto>();
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

            HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync("api/auth/cambiar-clave-temporal", request);

            if (!respuesta.IsSuccessStatusCode)
            {
                string mensaje = await ObtenerMensajeErrorAsync(respuesta);
                throw new ApiException(mensaje);
            }

            var resultado = await respuesta.Content.ReadFromJsonAsync<LoginApiResponseDto>();
            return resultado ?? throw new ApiException("Respuesta vacía del servidor.");
        }

        private static async Task<string> ObtenerMensajeErrorAsync(HttpResponseMessage respuesta)
        {
            try
            {
                var error = await respuesta.Content.ReadFromJsonAsync<ApiMensajeDto>();
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
