namespace IndexadorIA.Entidades
{
    public class Rotacion
    {
        public int CdRotacion { get; set; }
        public int CdArchivoPagina { get; set; }
        public string SnRotacionAutomatica { get; set; } = "NO";
        public int NuRotacionAplicada { get; set; }
        public string SnRotacionManual { get; set; } = "NO";
        public DateTime FeRotacion { get; set; }
        public int CdUsuario { get; set; }

        // Propiedades adicionales para UI
        public string DsArchivoPagina { get; set; } = string.Empty;
        public string DsUsuario { get; set; } = string.Empty;
    }
}
