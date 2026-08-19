using IndexadorIA.Api.Dtos;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    [ApiController]
    [Route("api/resultados")]
    [Authorize]
    public class ResultadosController : ControllerBase
    {
        private readonly ResultadoIADAL _resultadoIADAL;

        public ResultadosController()
        {
            _resultadoIADAL = new ResultadoIADAL();
        }

        private int ObtenerCdUsuarioActual()
        {
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        }

        /// <summary>
        /// Actualiza los datos corregidos de un resultado IA.
        /// Equivalente remoto de la corrección de datos en FrmVerLote (btnGuardarControlada_Click).
        /// </summary>
        [HttpPut("{cdResultado:int}/datos")]
        public IActionResult ActualizarDatos(int cdResultado, [FromBody] ActualizarResultadoRequestDto request)
        {
            var resultado = new ResultadoIA
            {
                CdResultado = cdResultado,
                CdCategoriaPlano = request.CdCategoriaPlano,
                CdTipoPlano = request.CdTipoPlano,
                DsNumeroPlano = request.DsNumeroPlano,
                DsExpediente = request.DsExpediente,
                DsSeccion = request.DsSeccion,
                DsManzana = request.DsManzana,
                DsParcela = request.DsParcela,
                DsDireccion = request.DsDireccion
            };

            _resultadoIADAL.ActualizarDatos(resultado, ObtenerCdUsuarioActual());

            return Ok(new { mensaje = "Datos actualizados correctamente" });
        }

        /// <summary>
        /// Cambia el estado de control de un resultado (Controlado, Página Ilegible, Datos Ilegibles).
        /// Equivalente remoto de FrmVerLote.MarcarEstadoControl(...).
        /// </summary>
        [HttpPut("{cdResultado:int}/estado-control")]
        public IActionResult ActualizarEstadoControl(int cdResultado, [FromBody] ActualizarEstadoControlRequestDto request)
        {
            _resultadoIADAL.ActualizarEstadoControl(
                cdResultado, request.CdEstadoControl, request.SnModificaDatos, ObtenerCdUsuarioActual());

            return Ok(new { mensaje = "Estado de control actualizado correctamente" });
        }
    }
}
