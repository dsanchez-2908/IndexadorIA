namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Fila del reporte "Producción x Usuarios": resume, para un usuario y un rango de
    /// fechas, la cantidad de resultados controlados, la cantidad de lotes que finalizó
    /// de control, y la cantidad de campos corregidos manualmente.
    /// </summary>
    public class ProduccionUsuarioDto
    {
        public int CdUsuario { get; set; }
        public string DsUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Fecha del detalle diario. Es null cuando el reporte se muestra totalizado
        /// (sin el filtro "Detalle por Fecha").
        /// </summary>
        public DateTime? Fecha { get; set; }
        public int CantidadControlada { get; set; }
        public int CantidadLotesCompletos { get; set; }
        public int CantidadCamposCorregidos { get; set; }
    }
}
