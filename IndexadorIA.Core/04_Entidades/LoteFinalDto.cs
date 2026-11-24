namespace IndexadorIA.Entidades
{
    /// <summary>Archivo original elegible para un lote final (fila de la grilla Crear Lotes Final).</summary>
    public class ArchivoOriginalElegibleDto
    {
        public bool Seleccionado { get; set; }
        public int CdArchivo { get; set; }
        public string DsNombreArchivo { get; set; } = string.Empty;
        public string DsRutaCompleta { get; set; } = string.Empty;
        public int NuPaginas { get; set; }
        public int NuCorrectos { get; set; }
        public int NuIlegibles { get; set; }
    }

    public class LoteFinalDto
    {
        public int CdLoteFinal { get; set; }
        public string DsNombreLote { get; set; } = string.Empty;
        public int CdUsuarioAsignado { get; set; }
        public string DsUsuarioAsignado { get; set; } = string.Empty;
        public int CdEstadoLote { get; set; }
        public string DsEstadoLote => CdEstadoLote == 2 ? "Finalizado" : "Asignado";
        public int NuArchivos { get; set; }
        public int NuPendientes { get; set; }
        public int NuFinalizados { get; set; }
        public DateTime FeAlta { get; set; }
    }
}
