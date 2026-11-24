namespace IndexadorIA.Entidades
{
    /// <summary>Registro pendiente de validar contra la API SMP.</summary>
    public class SmpPendiente
    {
        public int CdResultado { get; set; }
        public int? CdLote { get; set; }
        public int? CdArchivoPagina { get; set; }
        public string? DsSeccion { get; set; }
        public string? DsManzana { get; set; }
        public string? DsParcela { get; set; }
        public string? DsSmp { get; set; }
        public string? DsMotivoOmision { get; set; }
    }

    /// <summary>Resultado completo de una consulta a la API SMP (o de una omision).</summary>
    public class SmpValidacion : SmpPendiente
    {
        public string DsEstado { get; set; } = string.Empty;
        public string? DsMensaje { get; set; }
        public int? NuHttpStatus { get; set; }
        public string? DsJson { get; set; }
        public DateTime? FeConsulta { get; set; }
        public List<SmpParcelaVigente> Parcelas { get; set; } = new();
    }

    public class SmpParcelaVigente
    {
        public string DsSmp { get; set; } = string.Empty;
        public string? DsSurgeDe { get; set; }
        public DateTime? FeRegistro { get; set; }
        public string? DsCaso { get; set; }
        public string? DsObservacion { get; set; }
        public string? DsDirecciones { get; set; }
    }

    public class SmpContadores
    {
        public int Total { get; set; }
        public int Validados { get; set; }
        public int Pendientes { get; set; }
        public int Omitidos { get; set; }
        public int SinSincronizar { get; set; }
    }

    public static class SmpEstado
    {
        public const string Pendiente = "PENDIENTE";
        public const string Omitido = "OMITIDO";
        public const string Error = "error";
    }
}
