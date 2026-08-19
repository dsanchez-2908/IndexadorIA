namespace IndexadorIA.Negocio.Api
{
    /// <summary>
    /// DTOs livianos usados por el cliente WinForms para comunicarse con IndexadorIA.Api,
    /// espejo de los contratos definidos en IndexadorIA.Api.Dtos (sin acoplar el proyecto
    /// WinForms al proyecto Api).
    /// </summary>
    public class LoginApiRequestDto
    {
        public string DsUsuario { get; set; } = string.Empty;
        public string DsClave { get; set; } = string.Empty;
    }

    public class LoginApiResponseDto
    {
        public bool RequiereCambioClave { get; set; }
        public string? Token { get; set; }
        public int CdUsuario { get; set; }
        public string DsUsuario { get; set; } = string.Empty;
        public string DsNombreCompleto { get; set; } = string.Empty;
        public string CdRol { get; set; } = string.Empty;
        public string DsRol { get; set; } = string.Empty;
    }

    public class CambiarClaveTemporalApiRequestDto
    {
        public string DsUsuario { get; set; } = string.Empty;
        public string ClaveTemporal { get; set; } = string.Empty;
        public string NuevaClave { get; set; } = string.Empty;
        public string ConfirmarClave { get; set; } = string.Empty;
    }

    public class ApiMensajeDto
    {
        public string? Mensaje { get; set; }
    }
}
