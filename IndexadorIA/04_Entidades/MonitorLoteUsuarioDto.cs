namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO agregado por usuario para la grilla de la pantalla Monitor de Lotes:
    /// resume la cantidad de lotes y planos asignados/controlados/pendientes de cada usuario.
    /// </summary>
    public class MonitorLoteUsuarioDto
    {
        public int CdUsuario { get; set; }
        public string DsUsuario { get; set; } = string.Empty;

        public int NuTotalLotesAsignados { get; set; }
        public int NuCantidadPlanosAsignados { get; set; }

        public int NuTotalLotesControlados { get; set; }
        public int NuCantidadPlanosControlados { get; set; }

        public int NuTotalLotesPendientes { get; set; }
        public int NuCantidadPlanosPendientes { get; set; }
    }
}
