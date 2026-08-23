using System.Configuration;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Clase para gestionar la configuración de la aplicación
    /// </summary>
    public static class Configuracion
    {
        private static string? _cadenaConexion;

        /// <summary>
        /// Obtiene la cadena de conexión a la base de datos
        /// </summary>
        public static string CadenaConexion
        {
            get
            {
                if (string.IsNullOrEmpty(_cadenaConexion))
                {
                    // Intenta leer desde App.config, si no existe usa la cadena por defecto
                    try
                    {
                        _cadenaConexion = ConfigurationManager.ConnectionStrings["IndexadorIA"]?.ConnectionString;
                    }
                    catch
                    {
                        _cadenaConexion = null;
                    }

                    // Si no se encontró en el config, dejar vacío
                    if (string.IsNullOrEmpty(_cadenaConexion))
                    {
                        _cadenaConexion = string.Empty;
                    }
                }
                return _cadenaConexion;
            }
            set
            {
                _cadenaConexion = value;
            }
        }

        /// <summary>
        /// Obtiene el nombre de la base de datos configurada, extra�do de la cadena de conexi�n.
        /// �til para mostrar en la interfaz el entorno/ambiente al que est� conectado el usuario.
        /// </summary>
        public static string ObtenerNombreBaseDeDatos()
        {
            string cadena = CadenaConexion;

            if (string.IsNullOrEmpty(cadena))
            {
                return string.Empty;
            }

            foreach (string parte in cadena.Split(';'))
            {
                string trimmed = parte.Trim();

                if (trimmed.StartsWith("Database=", StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed["Database=".Length..].Trim();
                }

                if (trimmed.StartsWith("Initial Catalog=", StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed["Initial Catalog=".Length..].Trim();
                }
            }

            return string.Empty;
        }
    }
}
