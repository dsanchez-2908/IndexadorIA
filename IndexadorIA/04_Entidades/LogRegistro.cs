namespace IndexadorIA.Entidades
{
    public class LogRegistro
    {
        public long CdLog { get; set; }
        public string DsNivel { get; set; } = "INFO";
        public string? DsModulo { get; set; }
        public string DsMensaje { get; set; } = string.Empty;
        public string? DsExcepcion { get; set; }
        public int? CdUsuario { get; set; }
        public string? DsUsuario { get; set; }
        public DateTime FeRegistro { get; set; }

        // Niveles de log predefinidos
        public static class Niveles
        {
            public const string DEBUG = "DEBUG";
            public const string INFO = "INFO";
            public const string WARNING = "WARNING";
            public const string ERROR = "ERROR";
            public const string CRITICAL = "CRITICAL";
        }
    }
}
