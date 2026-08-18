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

                    // Si no se encontró en el config, usa la cadena de desarrollo por defecto
                    if (string.IsNullOrEmpty(_cadenaConexion))
                    {
                        _cadenaConexion = "Server=localhost\\SQLEXPRESS;Database=IndexadorIA;User Id=sa;Password=123;TrustServerCertificate=True;";
                    }
                }
                return _cadenaConexion;
            }
            set
            {
                _cadenaConexion = value;
            }
        }
    }
}
