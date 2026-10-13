using IndexadorIA.Datos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    /// <summary>
    /// Endpoints relacionados a TV_CALLES, usados por FrmVerLote en modo remoto para
    /// validar el campo Direccion.
    /// </summary>
    [ApiController]
    [Route("api/calles")]
    [Authorize]
    public class CallesController : ControllerBase
    {
        private readonly CalleDAL _calleDAL;

        public CallesController()
        {
            _calleDAL = new CalleDAL();
        }

        /// <summary>
        /// Devuelve el listado completo de nombres de calles (nom_mapa) de TV_CALLES.
        /// Equivalente remoto de CalleDAL.ObtenerNombresCalles().
        /// </summary>
        [HttpGet("nombres")]
        public IActionResult ObtenerNombres()
        {
            var nombres = _calleDAL.ObtenerNombresCalles();
            return Ok(nombres);
        }
    }
}
