using IndexadorIA.Entidades;
using System.Text.Json;

namespace IndexadorIA.Utilidades
{
    /// <summary>
    /// Gestiona la persistencia de la configuración de recorte de imágenes
    /// </summary>
    public static class ConfiguracionRecorteManager
    {
        private static readonly string ArchivoConfig = "ConfiguracionRecorte.json";
        private static readonly string CarpetaTemporal = "Archivos\\Temp";

        /// <summary>
        /// Obtiene la ruta del archivo de configuración
        /// </summary>
        private static string ObtenerRutaArchivoConfig()
        {
            string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(directorioEjecucion, ArchivoConfig);
        }

        /// <summary>
        /// Obtiene la ruta de la carpeta temporal
        /// </summary>
        private static string ObtenerRutaCarpetaTemporal()
        {
            string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
            string rutaCarpeta = Path.Combine(directorioEjecucion, CarpetaTemporal);

            // Crear carpeta si no existe
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            return rutaCarpeta;
        }

        /// <summary>
        /// Carga la configuración guardada. Si no existe, devuelve configuración por defecto.
        /// </summary>
        public static ConfiguracionRecorte CargarConfiguracion()
        {
            try
            {
                string rutaArchivo = ObtenerRutaArchivoConfig();

                if (File.Exists(rutaArchivo))
                {
                    string json = File.ReadAllText(rutaArchivo);
                    var config = JsonSerializer.Deserialize<ConfiguracionRecorte>(json);
                    return config ?? new ConfiguracionRecorte();
                }
            }
            catch (Exception)
            {
                // Si hay error al leer, devolver configuración por defecto
            }

            // Configuración por defecto
            return new ConfiguracionRecorte();
        }

        /// <summary>
        /// Guarda la configuración actual
        /// </summary>
        public static void GuardarConfiguracion(ConfiguracionRecorte config)
        {
            try
            {
                string rutaArchivo = ObtenerRutaArchivoConfig();

                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                };

                string json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(rutaArchivo, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar configuración: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Copia un PDF a la carpeta temporal y devuelve la ruta destino
        /// </summary>
        public static string CopiarPDFTemporal(string rutaOrigen)
        {
            try
            {
                if (!File.Exists(rutaOrigen))
                    throw new FileNotFoundException($"El archivo origen no existe: {rutaOrigen}");

                string carpetaTemporal = ObtenerRutaCarpetaTemporal();
                string nombreArchivo = Path.GetFileName(rutaOrigen);
                string rutaDestino = Path.Combine(carpetaTemporal, "preview_" + nombreArchivo);

                File.Copy(rutaOrigen, rutaDestino, true);

                return rutaDestino;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al copiar PDF temporal: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene la ruta del PDF temporal de la configuración actual
        /// </summary>
        public static string? ObtenerRutaPDFTemporal()
        {
            var config = CargarConfiguracion();

            if (!string.IsNullOrEmpty(config.RutaPDFTemporal) && File.Exists(config.RutaPDFTemporal))
            {
                return config.RutaPDFTemporal;
            }

            return null;
        }

        /// <summary>
        /// Limpia archivos temporales antiguos
        /// </summary>
        public static void LimpiarArchivosTemporales(int diasAntiguedad = 7)
        {
            try
            {
                string carpetaTemporal = ObtenerRutaCarpetaTemporal();

                if (!Directory.Exists(carpetaTemporal))
                    return;

                var archivos = Directory.GetFiles(carpetaTemporal);
                DateTime fechaLimite = DateTime.Now.AddDays(-diasAntiguedad);

                foreach (var archivo in archivos)
                {
                    var infoArchivo = new FileInfo(archivo);
                    if (infoArchivo.LastWriteTime < fechaLimite)
                    {
                        try
                        {
                            File.Delete(archivo);
                        }
                        catch
                        {
                            // Ignorar errores al eliminar archivos individuales
                        }
                    }
                }
            }
            catch
            {
                // Ignorar errores en limpieza
            }
        }
    }
}
