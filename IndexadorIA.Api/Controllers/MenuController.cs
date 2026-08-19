using IndexadorIA.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    [ApiController]
    [Route("api/menu")]
    [Authorize]
    public class MenuController : ControllerBase
    {
        /// <summary>
        /// Devuelve las pantallas habilitadas para el usuario autenticado, según su rol.
        /// Refleja las mismas restricciones aplicadas en FrmPrincipal.AplicarPermisosPorRol().
        /// </summary>
        [HttpGet("pantallas")]
        public IActionResult ObtenerPantallas()
        {
            string cdRol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;

            var pantallas = new List<PantallaDto>();

            if (cdRol == "DATAENTRY")
            {
                pantallas.Add(new PantallaDto { Clave = "CONTROL_FINALIZACION", Nombre = "Control y Finalización" });
                pantallas.Add(new PantallaDto { Clave = "CAMBIAR_CLAVE", Nombre = "Cambiar Clave" });
            }

            return Ok(pantallas);
        }
    }
}
