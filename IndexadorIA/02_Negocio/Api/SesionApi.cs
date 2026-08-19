namespace IndexadorIA.Negocio.Api
{
    /// <summary>
    /// Estado de sesión cuando la aplicación WinForms opera en modo remoto (consumiendo
    /// IndexadorIA.Api) en lugar de acceder directamente a la base de datos.
    /// </summary>
    public static class SesionApi
    {
        /// <summary>
        /// Indica si la sesión actual fue iniciada en modo remoto (API) o local (BD directa).
        /// </summary>
        public static bool ModoRemoto { get; set; }

        /// <summary>
        /// Token JWT devuelto por IndexadorIA.Api tras un login exitoso.
        /// </summary>
        public static string? Token { get; set; }

        public static void CerrarSesion()
        {
            ModoRemoto = false;
            Token = null;
        }
    }
}
