namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa un estado del sistema
    /// </summary>
    public class Estado
    {
        public int IdEstado { get; set; }
        public string DsProceso { get; set; } = string.Empty;
        public int CdEstado { get; set; }
        public string DsEstado { get; set; } = string.Empty;
    }
}
