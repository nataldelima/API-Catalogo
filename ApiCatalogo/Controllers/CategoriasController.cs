using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IUnitOfWork uow, ILogger<CategoriasController> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categoriaas = _uow.CategoriaRepository.GetAll();
            return Ok(categoriaas);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _uow.CategoriaRepository.Get(c=>c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id {id} não encontrada...");
                return NotFound($"Categoria com id {id} não encontrada...");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var categoriaCriada = _uow.CategoriaRepository.Create(categoria);
            _uow.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId },
                categoriaCriada);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            _uow.CategoriaRepository.Update(categoria);
            _uow.Commit();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _uow.CategoriaRepository.Get(c=> c.CategoriaId ==id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id {id} não localizada...");
                return NotFound($"Categoria com id {id} não localizada...");
            }

            var categoriaExcluida = _uow.CategoriaRepository.Delete(categoria);
            _uow.Commit();
            return Ok(categoriaExcluida);
        }
    }
}