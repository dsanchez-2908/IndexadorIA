namespace IndexadorIA.Entidades
{
    /// <summary>
    /// DTO con los indicadores del reporte "Estado Proyecto Planos": totales del
    /// proyecto, cantidad ingresada, estado de control, pendientes y asignaciones.
    /// </summary>
    public class EstadoProyectoDto
    {
        // Total de Proyecto Planos de GCBA
        public int TotalPlanosDelProyecto { get; set; }
        public int TotalPlanosPendientesDelProyecto { get; set; }

        // Ingresados hasta el momento
        public int TotalArchivosIngresados { get; set; }
        public int TotalPaginasIngresadas { get; set; }

        // Estado de Control
        public int TotalPlanosRevisados { get; set; }
        public int TotalPlanosControlados { get; set; }
        public int TotalPlanosFaltaDatos { get; set; }
        public int TotalPlanosIlegibles { get; set; }

        // Pendientes de Control
        public int TotalPlanosPendientesIngresados { get; set; }
        public int TotalPlanosPendientesProyecto { get; set; }

        // Asignaciones
        public int TotalPlanosAsignadosPendienteControl { get; set; }
        public int TotalPlanosSinAsignarControl { get; set; }
    }
}
