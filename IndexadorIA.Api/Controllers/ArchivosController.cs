using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    [ApiController]
    [Route("api/archivos")]
    [Authorize]
    public class ArchivosController : ControllerBase
    {
        private readonly LoteDAL _loteDAL;
        private readonly LogDAL _logDAL;
        private readonly ParametrosDAL _parametrosDAL;

        public ArchivosController()
        {
            _loteDAL = new LoteDAL();
            _logDAL = new LogDAL();
            _parametrosDAL = new ParametrosDAL();
        }

        /// <summary>
        /// Resuelve la ruta fisica que debe usar el proceso de la API para acceder a un archivo,
        /// reemplazando el prefijo de unidad mapeada con la que se guardo la ruta en BD (ej: "P:\")
        /// por el path UNC equivalente (ej: "\\FS-CLUSTER01\Recursos3\"), ya que el proceso de IIS
        /// (Application Pool) no tiene visibilidad de unidades de red mapeadas por sesion interactiva.
        /// El mapeo se toma de TD_PARAMETROS (RUTA_ORIGEN_UNIDAD / RUTA_DESTINO_UNC) para no requerir
        /// recompilar si cambia el servidor o el recurso compartido.
        /// </summary>
        private string ResolverRutaFisica(string rutaGuardada)
        {
            string? origen = _parametrosDAL.ObtenerValor("RUTA_ORIGEN_UNIDAD");
            string? destino = _parametrosDAL.ObtenerValor("RUTA_DESTINO_UNC");

            if (string.IsNullOrWhiteSpace(origen) || string.IsNullOrWhiteSpace(destino))
                return rutaGuardada;

            if (rutaGuardada.StartsWith(origen, StringComparison.OrdinalIgnoreCase))
                return destino + rutaGuardada.Substring(origen.Length);

            return rutaGuardada;
        }

        /// <summary>
        /// Devuelve los bytes del PDF de un archivo/página, para el botón
        /// "Ver Imagen PDF" remoto de FrmVerLote.
        /// </summary>
        [HttpGet("lotes/{cdLote:int}/paginas/{cdArchivoPagina:int}/pdf")]
        public IActionResult ObtenerPdf(int cdLote, int cdArchivoPagina)
        {
            var archivo = _loteDAL.ObtenerArchivosPaginasPorLote(cdLote)
                .FirstOrDefault(a => a.CdArchivoPagina == cdArchivoPagina);

            if (archivo == null)
                return NotFound(new { mensaje = "Archivo no encontrado en el lote indicado" });

            string rutaPdf = ResolverRutaFisica(archivo.DsRutaCompleta);

            if (!System.IO.File.Exists(rutaPdf))
            {
                RegistrarErrorArchivoNoEncontrado("ObtenerPdf", cdLote, cdArchivoPagina, rutaPdf);
                return NotFound(new { mensaje = "El archivo PDF no existe en el servidor", ruta = rutaPdf });
            }

            byte[] bytes = System.IO.File.ReadAllBytes(rutaPdf);
            string nombreArchivo = archivo.DsNombreArchivoPagina;

            return File(bytes, "application/pdf", nombreArchivo);
        }

        /// <summary>
        /// Devuelve los bytes del recorte JPG de un archivo/p&#225;gina, para el visor
        /// embebido remoto de FrmVerLote.
        /// </summary>
        [HttpGet("lotes/{cdLote:int}/paginas/{cdArchivoPagina:int}/jpg")]
        public IActionResult ObtenerJpg(int cdLote, int cdArchivoPagina)
        {
            var archivo = _loteDAL.ObtenerArchivosPaginasPorLote(cdLote)
                .FirstOrDefault(a => a.CdArchivoPagina == cdArchivoPagina);

            if (archivo == null)
                return NotFound(new { mensaje = "Archivo no encontrado en el lote indicado" });

            string rutaJpg = System.IO.Path.ChangeExtension(ResolverRutaFisica(archivo.DsRutaCompleta), ".jpg");

            if (!System.IO.File.Exists(rutaJpg))
            {
                RegistrarErrorArchivoNoEncontrado("ObtenerJpg", cdLote, cdArchivoPagina, rutaJpg);
                return NotFound(new { mensaje = "El archivo JPG no existe en el servidor", ruta = rutaJpg });
            }

            byte[] bytes = System.IO.File.ReadAllBytes(rutaJpg);
            string nombreArchivo = System.IO.Path.ChangeExtension(archivo.DsNombreArchivoPagina, ".jpg");

            return File(bytes, "image/jpeg", nombreArchivo);
        }

        /// <summary>
        /// Registra en TD_LOGS el detalle de un archivo no encontrado por el proceso de la API,
        /// incluyendo la ruta buscada, la identidad del proceso (Application Pool) y si la
        /// raiz/unidad y el directorio padre de la ruta son visibles para ese proceso. Permite
        /// diagnosticar diferencias de sesion/permisos entre el cliente WinForms y el proceso de IIS.
        /// </summary>
        private void RegistrarErrorArchivoNoEncontrado(string origen, int cdLote, int cdArchivoPagina, string ruta)
        {
            try
            {
                string identidadProceso = System.Security.Principal.WindowsIdentity.GetCurrent()?.Name ?? "(desconocida)";
                string raiz = System.IO.Path.GetPathRoot(ruta) ?? string.Empty;
                bool raizExiste = !string.IsNullOrEmpty(raiz) && System.IO.Directory.Exists(raiz);
                string? directorioPadre = System.IO.Path.GetDirectoryName(ruta);
                bool directorioPadreExiste = !string.IsNullOrEmpty(directorioPadre) && System.IO.Directory.Exists(directorioPadre);

                string mensaje = $"[{origen}] Archivo no encontrado. cdLote={cdLote}, cdArchivoPagina={cdArchivoPagina}, " +
                    $"ruta='{ruta}', raiz='{raiz}' (existe={raizExiste}), directorioPadre='{directorioPadre}' (existe={directorioPadreExiste}), " +
                    $"identidadProceso='{identidadProceso}', maquina='{Environment.MachineName}'";

                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "ArchivosController." + origen,
                    DsMensaje = mensaje
                });
            }
            catch
            {
                // Si falla el registro de log, no debe impedir la respuesta NotFound original.
            }
        }
    }
}
