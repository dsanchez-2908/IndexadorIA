using IndexadorIA.Api.Dtos;
using IndexadorIA.Api.Servicios;
using IndexadorIA.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioBL _usuarioBL;
        private readonly JwtServicio _jwtServicio;

        public AuthController(JwtServicio jwtServicio)
        {
            _usuarioBL = new UsuarioBL();
            _jwtServicio = jwtServicio;
        }

        /// <summary>
        /// Valida credenciales. Si el usuario tiene clave temporal o es su primer ingreso,
        /// no se emite token: el cliente debe llamar a /api/auth/cambiar-clave-temporal.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var (exito, mensaje, usuario) = _usuarioBL.ValidarLogin(request.DsUsuario, request.DsClave);

            if (!exito || usuario == null)
                return Unauthorized(new { mensaje });

            if (usuario.CdRol != "DATAENTRY")
                return Forbid();

            if (usuario.SnClaveTemporal || usuario.SnPrimerIngreso)
            {
                return Ok(new LoginResponseDto
                {
                    RequiereCambioClave = true,
                    CdUsuario = usuario.CdUsuario,
                    DsUsuario = usuario.DsUsuario,
                    DsNombreCompleto = usuario.DsNombreCompleto,
                    CdRol = usuario.CdRol,
                    DsRol = usuario.DsRol
                });
            }

            string token = _jwtServicio.GenerarToken(usuario);

            return Ok(new LoginResponseDto
            {
                RequiereCambioClave = false,
                Token = token,
                CdUsuario = usuario.CdUsuario,
                DsUsuario = usuario.DsUsuario,
                DsNombreCompleto = usuario.DsNombreCompleto,
                CdRol = usuario.CdRol,
                DsRol = usuario.DsRol
            });
        }

        /// <summary>
        /// Cambia la clave temporal (primer ingreso o restablecida) y, si tiene éxito,
        /// devuelve directamente el token JWT para continuar la sesión.
        /// </summary>
        [HttpPost("cambiar-clave-temporal")]
        [AllowAnonymous]
        public IActionResult CambiarClaveTemporal([FromBody] CambiarClaveTemporalRequestDto request)
        {
            var (exitoLogin, mensajeLogin, usuario) = _usuarioBL.ValidarLogin(request.DsUsuario, request.ClaveTemporal);

            if (!exitoLogin || usuario == null)
                return Unauthorized(new { mensaje = mensajeLogin });

            var (exito, mensaje) = _usuarioBL.CambiarClavePrimerIngreso(usuario.CdUsuario, request.NuevaClave, request.ConfirmarClave);

            if (!exito)
                return BadRequest(new { mensaje });

            string token = _jwtServicio.GenerarToken(usuario);

            return Ok(new LoginResponseDto
            {
                RequiereCambioClave = false,
                Token = token,
                CdUsuario = usuario.CdUsuario,
                DsUsuario = usuario.DsUsuario,
                DsNombreCompleto = usuario.DsNombreCompleto,
                CdRol = usuario.CdRol,
                DsRol = usuario.DsRol
            });
        }

        /// <summary>
        /// Cambia la clave de un usuario ya autenticado (sesión normal, no temporal).
        /// </summary>
        [HttpPost("cambiar-clave")]
        [Authorize]
        public IActionResult CambiarClave([FromBody] CambiarClaveRequestDto request)
        {
            int cdUsuario = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            var (exito, mensaje) = _usuarioBL.CambiarClave(cdUsuario, request.ClaveActual, request.NuevaClave, request.ConfirmarClave);

            if (!exito)
                return BadRequest(new { mensaje });

            return Ok(new { mensaje });
        }
    }
}
