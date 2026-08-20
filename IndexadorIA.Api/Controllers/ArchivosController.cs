using IndexadorIA.Datos;
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

        public ArchivosController()
        {
            _loteDAL = new LoteDAL();
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

            if (!System.IO.File.Exists(archivo.DsRutaCompleta))
                return NotFound(new { mensaje = "El archivo PDF no existe en el servidor" });

            byte[] bytes = System.IO.File.ReadAllBytes(archivo.DsRutaCompleta);
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

            string rutaJpg = System.IO.Path.ChangeExtension(archivo.DsRutaCompleta, ".jpg");

            if (!System.IO.File.Exists(rutaJpg))
                return NotFound(new { mensaje = "El archivo JPG no existe en el servidor" });

            byte[] bytes = System.IO.File.ReadAllBytes(rutaJpg);
            string nombreArchivo = System.IO.Path.ChangeExtension(archivo.DsNombreArchivoPagina, ".jpg");

            return File(bytes, "image/jpeg", nombreArchivo);
        }
    }
}
