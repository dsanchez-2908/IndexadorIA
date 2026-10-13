using IndexadorIA.Api.Dtos;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    /// <summary>
    /// Endpoints para la pantalla de Consulta de Planos (FrmConsultaPlanos) cuando el
    /// usuario esta logueado en modo Remoto.
    /// </summary>
    [ApiController]
    [Route("api/planos-consulta")]
    [Authorize]
    public class PlanoConsultaController : ControllerBase
    {
        private const string ClaveParametroMaxConsulta = "MAX_CONSULTA";
        private const int MaxRegistrosResultadoPorDefecto = 50000;

        private readonly PlanoConsultaDAL _planoConsultaDAL;
        private readonly ParametrosDAL _parametrosDAL;

        public PlanoConsultaController()
        {
            _planoConsultaDAL = new PlanoConsultaDAL();
            _parametrosDAL = new ParametrosDAL();
        }

        /// <summary>
        /// Consulta la vista VW_PLANOS_CONSULTAS aplicando los filtros indicados.
        /// Equivalente remoto de FrmConsultaPlanos.Buscar().
        /// </summary>
        [HttpPost("buscar")]
        public IActionResult Buscar([FromBody] PlanoConsultaFiltroDto filtroDto)
        {
            var filtro = new PlanoConsultaFiltro
            {
                CdLote = filtroDto.CdLote,
                CdEstadoLote = filtroDto.CdEstadoLote,
                DsDescripcionLote = filtroDto.DsDescripcionLote,
                DsEntrega = filtroDto.DsEntrega,
                DsCarpeta = filtroDto.DsCarpeta,
                CdEstadoControl = filtroDto.CdEstadoControl,
                CdCategoriaPlano = filtroDto.CdCategoriaPlano,
                CdTipoPlano = filtroDto.CdTipoPlano,
                DsExpediente = filtroDto.DsExpediente,
                DsSeccion = filtroDto.DsSeccion,
                DsManzana = filtroDto.DsManzana,
                DsParcela = filtroDto.DsParcela,
                DsDireccion = filtroDto.DsDireccion,
                DsNumeroPlano = filtroDto.DsNumeroPlano,
                DsObservaciones = filtroDto.DsObservaciones,
                MaxRegistros = filtroDto.MaxRegistros > 0 ? filtroDto.MaxRegistros : MaxRegistrosResultadoPorDefecto
            };

            var resultado = _planoConsultaDAL.Consultar(filtro);
            return Ok(resultado.Select(PlanoConsultaDto.DesdeEntidad));
        }

        /// <summary>
        /// Estados de lote (dsProceso='LOTE') utilizados como filtro.
        /// Equivalente remoto de PlanoConsultaDAL.ObtenerEstadosLoteParaFiltro().
        /// </summary>
        [HttpGet("estados-lote")]
        public IActionResult ObtenerEstadosLote()
        {
            var estados = _planoConsultaDAL.ObtenerEstadosLoteParaFiltro();
            return Ok(estados.Select(e => new EstadoLoteFiltroDto { CdEstado = e.Key, DsEstado = e.Value }));
        }

        /// <summary>
        /// Estados de control (dsProceso='CONTROL') utilizados como filtro.
        /// Equivalente remoto de PlanoConsultaDAL.ObtenerEstadosControlParaFiltro().
        /// </summary>
        [HttpGet("estados-control")]
        public IActionResult ObtenerEstadosControl()
        {
            var estados = _planoConsultaDAL.ObtenerEstadosControlParaFiltro();
            return Ok(estados.Select(e => new EstadoLoteFiltroDto { CdEstado = e.Key, DsEstado = e.Value }));
        }

        /// <summary>
        /// Valor del parametro MAX_CONSULTA (o el valor por defecto si no esta
        /// configurado), usado para limitar la cantidad de registros mostrados.
        /// Equivalente remoto de FrmConsultaPlanos.CargarMaxRegistrosResultado().
        /// </summary>
        [HttpGet("max-registros")]
        public IActionResult ObtenerMaxRegistros()
        {
            string? valor = _parametrosDAL.ObtenerValor(ClaveParametroMaxConsulta);
            int maxRegistros = int.TryParse(valor, out int parsed) && parsed > 0
                ? parsed
                : MaxRegistrosResultadoPorDefecto;

            return Ok(new { maxRegistros });
        }
    }
}
