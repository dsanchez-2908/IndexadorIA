namespace IndexadorIA.Api.Dtos
{
    public class LoginRequestDto
    {
        public string DsUsuario { get; set; } = string.Empty;
        public string DsClave { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public bool RequiereCambioClave { get; set; }
        public string? Token { get; set; }
        public int CdUsuario { get; set; }
        public string DsUsuario { get; set; } = string.Empty;
        public string DsNombreCompleto { get; set; } = string.Empty;
        public string CdRol { get; set; } = string.Empty;
        public string DsRol { get; set; } = string.Empty;
    }

    public class CambiarClaveTemporalRequestDto
    {
        public string DsUsuario { get; set; } = string.Empty;
        public string ClaveTemporal { get; set; } = string.Empty;
        public string NuevaClave { get; set; } = string.Empty;
        public string ConfirmarClave { get; set; } = string.Empty;
    }

    public class CambiarClaveRequestDto
    {
        public string ClaveActual { get; set; } = string.Empty;
        public string NuevaClave { get; set; } = string.Empty;
        public string ConfirmarClave { get; set; } = string.Empty;
    }
}
