using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// Clase para almacenar la sesión del usuario actual
    /// </summary>
    public static class SesionActual
    {
        public static Usuario? UsuarioActual { get; set; }

        public static bool EstaAutenticado => UsuarioActual != null;

        public static void CerrarSesion()
        {
            UsuarioActual = null;
        }
    }
}
