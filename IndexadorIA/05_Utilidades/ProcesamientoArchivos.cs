using System.Drawing;
using IndexadorIA.Entidades;
using PdfiumViewer;

namespace IndexadorIA.Utilidades
{
    public class ProcesamientoArchivos
    {
        public class ResultadoArchivo
        {
            public string NombreArchivo { get; set; } = string.Empty;
            public string Extension { get; set; } = string.Empty;
            public string RutaCompleta { get; set; } = string.Empty;
            public string? NombreUltimaCarpeta { get; set; }
            public int CantidadPaginas { get; set; }
            public long TamanoBytes { get; set; }
            public DateTime FeModificacion { get; set; }
            public string? Error { get; set; }
        }

        public static List<ResultadoArchivo> BuscarArchivos(string rutaOrigen, bool incluirSubcarpetas, IProgress<string>? progreso = null)
        {
            var resultados = new List<ResultadoArchivo>();
            var extensionesValidas = new[] { ".pdf", ".jpg", ".jpeg" };

            try
            {
                var opcionBusqueda = incluirSubcarpetas ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var archivos = Directory.GetFiles(rutaOrigen, "*.*", opcionBusqueda)
                    .Where(f => extensionesValidas.Contains(Path.GetExtension(f).ToLower()))
                    .ToList();

                progreso?.Report($"Se encontraron {archivos.Count} archivos. Procesando...");

                int contador = 0;
                foreach (var archivo in archivos)
                {
                    contador++;
                    progreso?.Report($"Procesando archivo {contador}/{archivos.Count}: {Path.GetFileName(archivo)}");

                    var resultado = ProcesarArchivo(archivo);
                    resultados.Add(resultado);
                }

                progreso?.Report($"Procesamiento completado. Total: {resultados.Count} archivos.");
            }
            catch (Exception ex)
            {
                progreso?.Report($"Error al buscar archivos: {ex.Message}");
            }

            return resultados;
        }

        private static ResultadoArchivo ProcesarArchivo(string rutaCompleta)
        {
            var extension = Path.GetExtension(rutaCompleta);

            var resultado = new ResultadoArchivo
            {
                NombreArchivo = Path.GetFileName(rutaCompleta), // Nombre completo con extensión
                Extension = extension.TrimStart('.'), // Extensión sin el punto
                RutaCompleta = rutaCompleta,
                NombreUltimaCarpeta = Path.GetFileName(Path.GetDirectoryName(rutaCompleta))
            };

            try
            {
                var fileInfo = new FileInfo(rutaCompleta);
                resultado.TamanoBytes = fileInfo.Length;
                resultado.FeModificacion = fileInfo.LastWriteTime;

                // Determinar cantidad de páginas
                string extensionLower = extension.ToLower();

                if (extensionLower == ".pdf")
                {
                    resultado.CantidadPaginas = ContarPaginasPdf(rutaCompleta);
                }
                else if (extensionLower == ".jpg" || extensionLower == ".jpeg")
                {
                    resultado.CantidadPaginas = 1; // Las imágenes JPG son siempre 1 página
                }
            }
            catch (Exception ex)
            {
                resultado.Error = ex.Message;
                resultado.CantidadPaginas = 0;
            }

            return resultado;
        }

        private static int ContarPaginasPdf(string rutaPdf)
        {
            try
            {
                using (var document = PdfDocument.Load(rutaPdf))
                {
                    return document.PageCount;
                }
            }
            catch (Exception ex)
            {
                // Si hay error al cargar el PDF, registramos el error pero devolvemos 0
                System.Diagnostics.Debug.WriteLine($"Error al contar páginas de {rutaPdf}: {ex.Message}");
                return 0;
            }
        }

        public static ArchivoOriginal ConvertirAArchivoOriginal(ResultadoArchivo resultado, int cdProyecto, int cdUsuarioAlta, int cdEstadoArchivo)
        {
            return new ArchivoOriginal
            {
                CdProyecto = cdProyecto,
                DsNombreArchivo = resultado.NombreArchivo,
                DsExtension = resultado.Extension,
                DsRutaCompleta = resultado.RutaCompleta,
                DsNombreUltimaCarpeta = resultado.NombreUltimaCarpeta,
                NuCantidadPaginas = resultado.CantidadPaginas,
                CdEstadoArchivo = cdEstadoArchivo,
                CdUsuarioAlta = cdUsuarioAlta,
                NuTamanoBytes = resultado.TamanoBytes,
                FeModificacionArchivo = resultado.FeModificacion
            };
        }
    }
}
