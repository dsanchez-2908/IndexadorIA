using IndexadorIA.Api.Dtos;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    [ApiController]
    [Route("api/lotes")]
    [Authorize]
    public class LotesController : ControllerBase
    {
        // Estado de lote "Controlando" (dsProceso = 'LOTE' en TD_ESTADOS)
        private const int CdEstadoControlando = 6;

        private readonly LoteDAL _loteDAL;
        private readonly ResultadoIADAL _resultadoIADAL;

        public LotesController()
        {
            _loteDAL = new LoteDAL();
            _resultadoIADAL = new ResultadoIADAL();
        }

        private int ObtenerCdUsuarioActual()
        {
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        }

        /// <summary>
        /// Lotes en estado "Controlando" asignados al usuario autenticado, con filtros opcionales.
        /// Equivalente remoto de FrmControlFinalizacion.CargarLotes().
        /// </summary>
        [HttpGet]
        public IActionResult ObtenerLotesEnControl(
            [FromQuery] string? dsNombreLote,
            [FromQuery] DateTime? feAltaDesde,
            [FromQuery] DateTime? feAltaHasta)
        {
            int cdUsuario = ObtenerCdUsuarioActual();

            List<Lote> lotes = _loteDAL.ObtenerLotesPorEstadoFiltrado(
                CdEstadoControlando, dsNombreLote, feAltaDesde, feAltaHasta, cdUsuario);

            return Ok(lotes.Select(LoteResumenDto.DesdeEntidad));
        }

        /// <summary>
        /// Encabezado y grilla de archivos/resultados de un lote.
        /// Equivalente remoto de FrmVerLote.CargarEncabezadoLote() + CargarDatosGrilla().
        /// </summary>
        [HttpGet("{cdLote:int}/detalle")]
        public IActionResult ObtenerDetalle(int cdLote)
        {
            Lote? lote = _loteDAL.ObtenerPorId(cdLote);
            if (lote == null)
                return NotFound(new { mensaje = "Lote no encontrado" });

            var archivos = _loteDAL.ObtenerArchivosPaginasPorLote(cdLote);
            var resultados = _resultadoIADAL.ObtenerPorLote(cdLote, mostrarTodos: true)
                .ToDictionary(r => r.CdArchivoPagina, r => r);

            var filas = archivos.Select(a => new FilaLoteDetalleDto
            {
                Archivo = ArchivoPaginaDto.DesdeEntidad(a),
                Resultado = resultados.TryGetValue(a.CdArchivoPagina, out var r) ? ResultadoIADto.DesdeEntidad(r) : null
            }).ToList();

            return Ok(new
            {
                Lote = LoteResumenDto.DesdeEntidad(lote),
                Filas = filas
            });
        }
    }
}
