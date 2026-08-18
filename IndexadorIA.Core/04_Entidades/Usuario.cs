namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un usuario del sistema
    /// </summary>
    public class Usuario
    {
        public int CdUsuario { get; set; }
        public string DsUsuario { get; set; } = string.Empty;
        public string DsClave { get; set; } = string.Empty;
        public string DsNombreCompleto { get; set; } = string.Empty;
        public bool SnClaveTemporal { get; set; }
        public bool SnPrimerIngreso { get; set; }
        public int IdRol { get; set; }
        public string CdRol { get; set; } = string.Empty;
        public string DsRol { get; set; } = string.Empty;
        public int CdEstado { get; set; }
        public string DsEstado { get; set; } = string.Empty;
        public DateTime FeAlta { get; set; }
        public int? CdUsuarioAlta { get; set; }
    }
}
