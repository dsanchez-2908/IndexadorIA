using IndexadorIA.Api.Dtos;
using IndexadorIA.Datos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndexadorIA.Api.Controllers
{
    [ApiController]
    [Route("api/combos")]
    [Authorize]
    public class CombosController : ControllerBase
    {
        private readonly CategoriaPlanoDAL _categoriaPlanoDAL;
        private readonly TipoPlanoDAL _tipoPlanoDAL;
        private readonly ReparticionDAL _reparticionDAL;

        public CombosController()
        {
            _categoriaPlanoDAL = new CategoriaPlanoDAL();
            _tipoPlanoDAL = new TipoPlanoDAL();
            _reparticionDAL = new ReparticionDAL();
        }

        [HttpGet("categorias-plano")]
        public IActionResult ObtenerCategoriasPlano()
        {
            var categorias = _categoriaPlanoDAL.ObtenerTodos()
                .Select(c => new CategoriaPlanoDto { CdCategoriaPlano = c.CdCategoriaPlano, DsCategoriaPlano = c.DsCategoriaPlano });

            return Ok(categorias);
        }

        [HttpGet("tipos-plano")]
        public IActionResult ObtenerTiposPlano()
        {
            var tipos = _tipoPlanoDAL.ObtenerTodos()
                .Select(t => new TipoPlanoDto { CdTipoPlano = t.CdTipoPlano, CdCategoriaPlano = t.CdCategoriaPlano, DsTipoPlano = t.DsTipoPlano });

            return Ok(tipos);
        }

        [HttpGet("reparticiones")]
        public IActionResult ObtenerReparticiones()
        {
            var reparticiones = _reparticionDAL.ObtenerTodos()
                .Select(r => new ReparticionDto { CdReparticion = r.CdReparticion, DsReparticion = r.DsReparticion });

            return Ok(reparticiones);
        }
    }
}
