namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Resumen de producción del usuario logueado, usado en el encabezado de
    /// FrmVerLote para mostrar el avance de control de planos.
    /// </summary>
    public class ResumenProduccionUsuario
    {
        public int CantidadProcesadosHoy { get; set; }
        public int CantidadAsignados { get; set; }
        public int CantidadProcesados { get; set; }
        public int CantidadPendientes { get; set; }
    }
}
