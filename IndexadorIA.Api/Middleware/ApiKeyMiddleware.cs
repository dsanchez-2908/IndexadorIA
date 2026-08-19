namespace IndexadorIA.Api.Middleware
{
    /// <summary>
    /// Valida que toda solicitud incluya el header "X-Api-Key" con el valor configurado
    /// en SeguridadApi:ApiKey. Se ejecuta antes de la autenticación JWT.
    /// </summary>
    public class ApiKeyMiddleware
    {
        private const string HeaderApiKey = "X-Api-Key";

        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuracion)
        {
            // Swagger y sus recursos estáticos quedan exentos para poder probar la API desde el navegador.
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            string? apiKeyConfigurada = configuracion["SeguridadApi:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKeyConfigurada))
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { mensaje = "La API Key no está configurada en el servidor." });
                return;
            }

            if (!context.Request.Headers.TryGetValue(HeaderApiKey, out var apiKeyRecibida) ||
                !string.Equals(apiKeyRecibida, apiKeyConfigurada, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { mensaje = "API Key inválida o faltante." });
                return;
            }

            await _next(context);
        }
    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKey(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
