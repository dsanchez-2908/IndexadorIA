using IndexadorIA.Api.Dtos;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    /// <summary>
    /// Expone el texto de ayuda de control configurado por el administrador para los
    /// 9 campos del panel de detalle de FrmVerLote. La edición se realiza únicamente
    /// desde el cliente Windows Forms (rol Administrador, modo local); este controller
    /// solo soporta lectura para que el modo remoto pueda mostrar la misma ayuda,
    /// y una escritura simple usada por la pantalla de administración cuando se
    /// conecta en modo remoto.
    /// </summary>
    [ApiController]
    [Route("api/ayuda-control")]
    [Authorize]
    public class AyudaControlController : ControllerBase
    {
        private readonly AyudaControlDAL _ayudaControlDAL;

        public AyudaControlController()
        {
            _ayudaControlDAL = new AyudaControlDAL();
        }

        [HttpGet]
        public IActionResult Obtener()
        {
            var ayuda = _ayudaControlDAL.Obtener();
            return Ok(AyudaControlDto.DesdeEntidad(ayuda));
        }

        [HttpPut]
        public IActionResult Guardar([FromBody] AyudaControlDto dto)
        {
            var ayuda = new AyudaControl
            {
                DsCategoriaPlano = dto.DsCategoriaPlano,
                DsTipoPlano = dto.DsTipoPlano,
                DsExpediente = dto.DsExpediente,
                DsSeccion = dto.DsSeccion,
                DsManzana = dto.DsManzana,
                DsParcela = dto.DsParcela,
                DsDireccion = dto.DsDireccion,
                DsNumeroPlano = dto.DsNumeroPlano,
                DsObservaciones = dto.DsObservaciones
            };

            _ayudaControlDAL.Guardar(ayuda);
            return Ok();
        }
    }
}
