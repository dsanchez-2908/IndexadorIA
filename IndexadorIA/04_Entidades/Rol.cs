namespace IndexadorIA.Entidades
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string CdRol { get; set; } = string.Empty;
        public string DsRol { get; set; } = string.Empty;
        public int CdEstado { get; set; }
        public string DsEstado { get; set; } = string.Empty;
    }
}
