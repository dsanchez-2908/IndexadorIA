using IndexadorIA.Api.Dtos;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    /// <summary>
    /// Endpoints para las pantallas de auditoria (FrmAuditar, FrmAuditarLote,
    /// FrmVerRegistroAuditoria) cuando el usuario esta logueado en modo Remoto.
    /// </summary>
    [ApiController]
    [Route("api/auditoria")]
    [Authorize]
    public class AuditoriaController : ControllerBase
    {
        // Estado de lote "Auditando" (dsProceso = 'LOTE' en TD_ESTADOS)
        private const int CdEstadoAuditando = 8;

        private readonly LoteDAL _loteDAL;
        private readonly ResultadoIADAL _resultadoIADAL;
        private readonly CorreccionDAL _correccionDAL;

        public AuditoriaController()
        {
            _loteDAL = new LoteDAL();
            _resultadoIADAL = new ResultadoIADAL();
            _correccionDAL = new CorreccionDAL();
        }

        private int ObtenerCdUsuarioActual()
        {
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        }

        /// <summary>
        /// Lotes en estado "Auditando" asignados al usuario auditor autenticado, con
        /// filtros opcionales. Equivalente remoto de FrmAuditar.CargarLotes().
        /// </summary>
        [HttpGet("lotes")]
        public IActionResult ObtenerLotes(
            [FromQuery] string? dsNombreLote,
            [FromQuery] DateTime? feAltaDesde,
            [FromQuery] DateTime? feAltaHasta)
        {
            int cdUsuario = ObtenerCdUsuarioActual();

            List<Lote> lotes = _loteDAL.ObtenerLotesPorEstadoFiltrado(
                CdEstadoAuditando, dsNombreLote, feAltaDesde, feAltaHasta,
                cdUsuarioAuditor: cdUsuario, filtrarFechaPorUltimoCambio: true);

            return Ok(lotes.Select(LoteAuditoriaDto.DesdeEntidad));
        }

        /// <summary>
        /// Encabezado de un lote en auditoria (nombre, usuario controlador, etc.).
        /// Equivalente remoto de FrmAuditarLote.CargarUsuarioControlador().
        /// </summary>
        [HttpGet("lotes/{cdLote:int}")]
        public IActionResult ObtenerLote(int cdLote)
        {
            Lote? lote = _loteDAL.ObtenerPorId(cdLote);
            if (lote == null)
                return NotFound(new { mensaje = "Lote no encontrado" });

            return Ok(LoteAuditoriaDto.DesdeEntidad(lote));
        }

        /// <summary>
        /// Registros de TD_001_RESULTADO_IA de un lote en auditoria, con filtro
        /// opcional por estados de control. Equivalente remoto de
        /// FrmAuditarLote.CargarRegistros().
        /// </summary>
        [HttpGet("lotes/{cdLote:int}/registros")]
        public IActionResult ObtenerRegistros(int cdLote, [FromQuery] string? cdEstadosControl)
        {
            List<int>? estados = null;
            if (!string.IsNullOrWhiteSpace(cdEstadosControl))
            {
                estados = cdEstadosControl
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();
            }

            var registros = _resultadoIADAL.ObtenerParaAuditoria(cdLote, estados);
            return Ok(registros.Select(RegistroAuditoriaDto.DesdeEntidad));
        }

        /// <summary>
        /// Estados de control (dsProceso='CONTROL') usados como filtro en la
        /// pantalla de auditoria. Equivalente remoto de
        /// ResultadoIADAL.ObtenerEstadosControlParaFiltro().
        /// </summary>
        [HttpGet("estados-control")]
        public IActionResult ObtenerEstadosControl()
        {
            var estados = _resultadoIADAL.ObtenerEstadosControlParaFiltro();
            return Ok(estados.Select(e => new EstadoControlDto { CdEstado = e.Key, DsEstado = e.Value }));
        }

        /// <summary>
        /// Posibles errores detectados por el stored procedure SP_CONTROL_PLANOS_cdLote
        /// para un lote. Equivalente remoto de FrmAuditarLote.btnAnalizar_Click.
        /// </summary>
        [HttpGet("lotes/{cdLote:int}/posibles-errores")]
        public IActionResult ObtenerPosiblesErrores(int cdLote)
        {
            var posiblesErrores = _resultadoIADAL.ObtenerPosiblesErrores(cdLote);

            var resultado = posiblesErrores.Select(kvp => new PosibleErrorAuditoriaDto
            {
                CdResultado = kvp.Key,
                PosiblesErrores = kvp.Value
            });

            return Ok(resultado);
        }

        /// <summary>
        /// Cantidad de registros Pendiente de Control (cdEstadoControl = 1) de un lote.
        /// Equivalente remoto de ResultadoIADAL.ContarPendientesControl(), usado por
        /// FrmAuditarLote.btnMarcarLoteAuditado_Click para bloquear la finalizacion de la
        /// auditoria mientras queden registros sin controlar.
        /// </summary>
        [HttpGet("lotes/{cdLote:int}/pendientes-control")]
        public IActionResult ObtenerCantidadPendientesControl(int cdLote)
        {
            int cantidad = _resultadoIADAL.ContarPendientesControl(cdLote);
            return Ok(new { cantidad });
        }

        /// <summary>
        /// Marca el lote como auditado. Equivalente remoto de
        /// FrmAuditarLote.btnMarcarLoteAuditado_Click.
        /// No permite finalizar la auditoria si quedan registros Pendiente de Control
        /// (cdEstadoControl = 1).
        /// </summary>
        [HttpPut("lotes/{cdLote:int}/marcar-auditado")]
        public IActionResult MarcarLoteAuditado(int cdLote)
        {
            int cantidadPendientes = _resultadoIADAL.ContarPendientesControl(cdLote);
            if (cantidadPendientes > 0)
                return BadRequest(new
                {
                    mensaje = $"No se puede finalizar la auditoria: quedan {cantidadPendientes} registro(s) Pendiente de Control."
                });

            int cdUsuario = ObtenerCdUsuarioActual();
            _loteDAL.MarcarLoteAuditado(cdLote, cdUsuario);

            return Ok(new { mensaje = "Lote marcado como auditado correctamente." });
        }

        /// <summary>
        /// Cantidad de registros Controlados (cdEstadoControl = 2) de un lote a los que
        /// les falta algun campo obligatorio. Equivalente remoto de
        /// ResultadoIADAL.ContarControladosConCamposIncompletos(), usado por
        /// FrmAuditarLote.btnMarcarLoteAuditado_Click para advertir al auditor antes de
        /// finalizar la auditoria.
        /// </summary>
        [HttpGet("lotes/{cdLote:int}/controlados-incompletos")]
        public IActionResult ObtenerCantidadControladosIncompletos(int cdLote)
        {
            int cantidad = _resultadoIADAL.ContarControladosConCamposIncompletos(cdLote);
            return Ok(new { cantidad });
        }

        /// <summary>
        /// Actualiza los datos corregidos de un resultado desde la pantalla de
        /// auditoria. Equivalente remoto de FrmVerRegistroAuditoria.btnGuardar_Click
        /// (actualizacion de datos).
        /// </summary>
        [HttpPut("resultados/{cdResultado:int}/datos")]
        public IActionResult ActualizarDatos(int cdResultado, [FromBody] ActualizarResultadoAuditoriaRequestDto request)
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
        /// Cambia el estado de control de un resultado desde la pantalla de auditoria.
        /// Equivalente remoto de FrmVerRegistroAuditoria.btnGuardar_Click (cambio de estado).
        /// </summary>
        [HttpPut("resultados/{cdResultado:int}/estado-control")]
        public IActionResult ActualizarEstadoControl(int cdResultado, [FromBody] ActualizarEstadoControlRequestDto request)
        {
            _resultadoIADAL.ActualizarEstadoControl(
                cdResultado, request.CdEstadoControl, request.SnModificaDatos, ObtenerCdUsuarioActual(), request.DsObservaciones);

            return Ok(new { mensaje = "Estado de control actualizado correctamente" });
        }

        /// <summary>
        /// Registra en TD_CORRECIONES las correcciones manuales realizadas por el
        /// auditor, dejando constancia en feAuditoria/cdUsuarioAuditoria.
        /// Equivalente remoto de FrmVerRegistroAuditoria.RegistrarCorreccionesDeAuditoriaSiCorresponde(...).
        /// </summary>
        [HttpPost("resultados/{cdResultado:int}/correcciones")]
        public IActionResult RegistrarCorrecciones(int cdResultado, [FromBody] RegistrarCorreccionesAuditoriaRequestDto request)
        {
            int cdUsuario = ObtenerCdUsuarioActual();

            foreach (var c in request.Correcciones)
            {
                _correccionDAL.Insertar(new Correccion
                {
                    CdResultado = cdResultado,
                    DsCampo = c.DsCampo,
                    DsValorAnterior = c.DsValorAnterior,
                    DsValorNuevo = c.DsValorNuevo,
                    CdUsuarioAuditoria = cdUsuario
                });
            }

            return Ok(new { mensaje = "Correcciones registradas correctamente" });
        }

        /// <summary>
        /// Propaga el cambio de dirección hecho al auditar un registro a todos los demás
        /// resultados del mismo lote que compartan el mismo expediente. Equivalente remoto
        /// de ResultadoIADAL.ActualizarDireccionPorExpedienteEnLote(...).
        /// </summary>
        [HttpPut("resultados/{cdResultado:int}/propagar-direccion")]
        public IActionResult PropagarDireccionPorExpediente(int cdResultado, [FromBody] PropagarDireccionAuditoriaRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.DsExpediente))
                return Ok(new { mensaje = "Sin expediente, no se propaga la dirección" });

            _resultadoIADAL.ActualizarDireccionPorExpedienteEnLote(
                request.CdLote, request.DsExpediente, cdResultado, request.DsDireccion, ObtenerCdUsuarioActual());

            return Ok(new { mensaje = "Dirección propagada correctamente" });
        }
    }
}
