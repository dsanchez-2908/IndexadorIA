using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    public class GuardarPaginaFinalRequest
    {
        public List<PaginaFinalDto> Paginas { get; set; } = new();
    }

    public class FinalizarPlanoFinalRequest
    {
        public List<int> CdArchivoPaginas { get; set; } = new();
    }

    public class FinalizarPlanoFinalResponse
    {
        public string NombreNuevo { get; set; } = string.Empty;
    }

    public class ReasignarLotesFinalesRequest
    {
        public List<int> CdLotes { get; set; } = new();
        public int CdUsuarioNuevo { get; set; }
    }

    /// <summary>
    /// Endpoints remotos de Finalizacion de Planos: Control Final (FrmControlFinal*)
    /// y Monitor de Lotes Finales (FrmMonitorLotesFinales*).
    /// </summary>
    [ApiController]
    [Route("api/lotes-finales")]
    [Authorize]
    public class LotesFinalesController : ControllerBase
    {
        private readonly LoteFinalControlDAL _dal = new();
        private readonly ParametrosDAL _parametrosDAL = new();

        private int CdUsuarioActual => int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        private bool EsAdmin => string.Equals(User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value, "ADMIN", StringComparison.OrdinalIgnoreCase);

        private bool PuedeOperarLote(int cdLoteFinal)
        {
            if (EsAdmin) return true;
            return _dal.ObtenerLotesPorUsuario(CdUsuarioActual).Any(l => l.CdLoteFinal == cdLoteFinal);
        }

        private string ResolverRutaFisica(string ruta)
        {
            string? origen = _parametrosDAL.ObtenerValor("RUTA_ORIGEN_UNIDAD");
            string? destino = _parametrosDAL.ObtenerValor("RUTA_DESTINO_UNC");
            if (string.IsNullOrWhiteSpace(origen) || string.IsNullOrWhiteSpace(destino)) return ruta;
            return ruta.StartsWith(origen, StringComparison.OrdinalIgnoreCase) ? destino + ruta.Substring(origen.Length) : ruta;
        }

        private string RestaurarRutaGuardada(string rutaFisica)
        {
            string? origen = _parametrosDAL.ObtenerValor("RUTA_ORIGEN_UNIDAD");
            string? destino = _parametrosDAL.ObtenerValor("RUTA_DESTINO_UNC");
            if (string.IsNullOrWhiteSpace(origen) || string.IsNullOrWhiteSpace(destino)) return rutaFisica;
            return rutaFisica.StartsWith(destino, StringComparison.OrdinalIgnoreCase) ? origen + rutaFisica.Substring(destino.Length) : rutaFisica;
        }

        // ---------- Control Final ----------

        [HttpGet("mis-lotes")]
        public IActionResult ObtenerMisLotes() => Ok(_dal.ObtenerLotesDeUsuario(CdUsuarioActual));

        [HttpGet("{cdLoteFinal:int}/archivos")]
        public IActionResult ObtenerArchivos(int cdLoteFinal)
        {
            if (!PuedeOperarLote(cdLoteFinal)) return Forbid();
            return Ok(_dal.ObtenerArchivosDeLote(cdLoteFinal));
        }

        [HttpGet("estados-control")]
        public IActionResult ObtenerEstadosControl() => Ok(_dal.ObtenerEstadosControl());

        [HttpGet("{cdLoteFinal:int}/archivos/{cdArchivo:int}/paginas")]
        public IActionResult ObtenerPaginas(int cdLoteFinal, int cdArchivo)
        {
            if (!PuedeOperarLote(cdLoteFinal)) return Forbid();
            return Ok(_dal.ObtenerPaginas(cdArchivo));
        }

        [HttpGet("{cdLoteFinal:int}/archivos/{cdArchivo:int}/paginas/{cdArchivoPagina:int}/pdf")]
        public IActionResult ObtenerPdf(int cdLoteFinal, int cdArchivo, int cdArchivoPagina)
        {
            if (!PuedeOperarLote(cdLoteFinal)) return Forbid();
            var pagina = _dal.ObtenerPaginas(cdArchivo).FirstOrDefault(p => p.CdArchivoPagina == cdArchivoPagina);
            if (pagina == null) return NotFound(new { mensaje = "Pagina no encontrada" });

            string ruta = ResolverRutaFisica(pagina.DsRutaPagina);
            if (!System.IO.File.Exists(ruta)) return NotFound(new { mensaje = "El archivo PDF no existe en el servidor" });
            return File(System.IO.File.ReadAllBytes(ruta), "application/pdf");
        }

        [HttpGet("resultados/{cdResultado:int}/validacion-smp")]
        public IActionResult ObtenerValidacionSmp(int cdResultado)
        {
            var v = _dal.ObtenerValidacionSmp(cdResultado);
            return v == null ? NoContent() : Ok(v);
        }

        [HttpPut("{cdLoteFinal:int}/paginas")]
        public IActionResult GuardarPaginas(int cdLoteFinal, [FromBody] GuardarPaginaFinalRequest request)
        {
            if (!PuedeOperarLote(cdLoteFinal)) return Forbid();
            int cdUsuario = CdUsuarioActual;
            foreach (var p in request.Paginas) _dal.GuardarPagina(p, cdUsuario);
            return NoContent();
        }

        /// <summary>Genera en el servidor el PDF final + CSV con las paginas indicadas y lo registra en BD.</summary>
        [HttpPost("{cdLoteFinal:int}/archivos/{cdArchivo:int}/finalizar-plano")]
        public IActionResult FinalizarPlano(int cdLoteFinal, int cdArchivo, [FromBody] FinalizarPlanoFinalRequest request)
        {
            if (!PuedeOperarLote(cdLoteFinal)) return Forbid();
            if (request.CdArchivoPaginas.Count == 0) return BadRequest(new { mensaje = "No hay paginas para finalizar." });

            var original = _dal.ObtenerArchivosDeLote(cdLoteFinal).FirstOrDefault(a => a.CdArchivo == cdArchivo);
            if (original == null) return NotFound(new { mensaje = "Archivo no encontrado en el lote" });

            var grupo = _dal.ObtenerPaginas(cdArchivo)
                .Where(p => request.CdArchivoPaginas.Contains(p.CdArchivoPagina))
                .OrderBy(p => p.NuPagina)
                .ToList();
            if (grupo.Count != request.CdArchivoPaginas.Distinct().Count())
                return BadRequest(new { mensaje = "Alguna pagina no existe o ya fue finalizada." });

            var m = grupo[0];
            if (!m.CdCategoriaPlano.HasValue) return BadRequest(new { mensaje = "Falta la categoria de plano." });

            foreach (var p in grupo) p.DsRutaPagina = ResolverRutaFisica(p.DsRutaPagina);
            var sinPdf = grupo.FirstOrDefault(p => !System.IO.File.Exists(p.DsRutaPagina));
            if (sinPdf != null) return BadRequest(new { mensaje = $"No se encontro el PDF de la pagina {sinPdf.NuPagina}." });

            FinalizadorPlanos.ResultadoFinal? res = null;
            try
            {
                res = FinalizadorPlanos.Generar(ResolverRutaFisica(original.DsRutaCompleta), original.DsNombreArchivo, grupo);
                _dal.RegistrarFinalizacion(cdLoteFinal, cdArchivo, grupo.Select(p => p.CdArchivoPagina),
                    res.NombreNuevo, RestaurarRutaGuardada(res.Carpeta), m.EsIlegible, CdUsuarioActual);
            }
            catch (Exception ex)
            {
                if (res != null) FinalizadorPlanos.Deshacer(res);
                return BadRequest(new { mensaje = ex.Message });
            }

            return Ok(new FinalizarPlanoFinalResponse { NombreNuevo = res.NombreNuevo });
        }

        [HttpPut("{cdLoteFinal:int}/archivos/{cdArchivo:int}/devolver-smp")]
        public IActionResult DevolverParaSmp(int cdLoteFinal, int cdArchivo)
        {
            if (!PuedeOperarLote(cdLoteFinal)) return Forbid();
            _dal.DevolverParaActualizarSmp(cdLoteFinal, cdArchivo);
            return NoContent();
        }

        // ---------- Monitor (solo ADMIN) ----------

        [HttpGet("monitor/resumen")]
        public IActionResult ObtenerResumen()
        {
            if (!EsAdmin) return Forbid();
            return Ok(_dal.ObtenerResumenPorUsuario());
        }

        [HttpGet("monitor/usuarios/{cdUsuario:int}/lotes")]
        public IActionResult ObtenerLotesPorUsuario(int cdUsuario)
        {
            if (!EsAdmin) return Forbid();
            return Ok(_dal.ObtenerLotesPorUsuario(cdUsuario));
        }

        [HttpGet("monitor/lotes/{cdLoteFinal:int}/archivos")]
        public IActionResult ObtenerArchivosMonitor(int cdLoteFinal)
        {
            if (!EsAdmin) return Forbid();
            return Ok(_dal.ObtenerArchivosDeLote(cdLoteFinal));
        }

        [HttpGet("monitor/usuarios")]
        public IActionResult ObtenerUsuarios()
        {
            if (!EsAdmin) return Forbid();
            var usuarios = new UsuarioDAL().ObtenerTodos().Select(u => new Usuario
            {
                CdUsuario = u.CdUsuario,
                DsUsuario = u.DsUsuario,
                DsNombreCompleto = u.DsNombreCompleto,
                CdRol = u.CdRol,
                CdEstado = u.CdEstado
            });
            return Ok(usuarios);
        }

        [HttpPut("monitor/reasignar")]
        public IActionResult Reasignar([FromBody] ReasignarLotesFinalesRequest request)
        {
            if (!EsAdmin) return Forbid();
            int n = _dal.Reasignar(request.CdLotes, request.CdUsuarioNuevo);
            return Ok(new { cantidad = n });
        }
    }
}
