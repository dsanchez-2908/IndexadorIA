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
        public IActionResult ObtenerDetalle(int cdLote, [FromQuery] bool mostrarTodos = true)
        {
            Lote? lote = _loteDAL.ObtenerPorId(cdLote);
            if (lote == null)
                return NotFound(new { mensaje = "Lote no encontrado" });

            var archivos = _loteDAL.ObtenerArchivosPaginasPorLote(cdLote);
            var resultados = _resultadoIADAL.ObtenerPorLote(cdLote, mostrarTodos)
                .ToDictionary(r => r.CdArchivoPagina, r => r);

            var filas = archivos.Select(a => new FilaLoteDetalleDto
            {
                Archivo = ArchivoPaginaDto.DesdeEntidad(a),
                Resultado = resultados.TryGetValue(a.CdArchivoPagina, out var r) ? ResultadoIADto.DesdeEntidad(r) : null
            }).ToList();

            var loteResumen = LoteResumenDto.DesdeEntidad(lote);
            loteResumen.FeProcesamientoIA = _loteDAL.ObtenerFechaProcesamientoIA(cdLote);

            return Ok(new
            {
                Lote = loteResumen,
                Filas = filas
            });
        }

        /// <summary>
        /// Marca el lote como "Pendiente de Finalizar". Equivalente remoto de
        /// FrmVerLote.btnMarcarLoteCompletado_Click (solo el cambio de estado, sin
        /// mover archivos, ya que eso se realiza desde la pantalla de finalizaci\u00F3n
        /// con acceso directo al almacenamiento).
        /// </summary>
        [HttpPut("{cdLote:int}/pendiente-finalizar")]
        public IActionResult MarcarPendienteFinalizar(int cdLote)
        {
            Lote? lote = _loteDAL.ObtenerPorId(cdLote);
            if (lote == null)
                return NotFound(new { mensaje = "Lote no encontrado" });

            int cdUsuario = ObtenerCdUsuarioActual();
            var loteFinalizacionBL = new IndexadorIA.Negocio.LoteFinalizacionBL();
            loteFinalizacionBL.MarcarLotePendienteFinalizar(cdLote, cdUsuario);

            return Ok(new { mensaje = "Lote marcado como Pendiente de Finalizar." });
        }
    }
}
