using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uow.CategoriaRepository.GetAll();
            if (categorias is null)
            {
                _logger.LogWarning("Não existem categorias...");
                return NotFound("Não existem categorias...");
            }

            var categoriasDto = categorias.ToCategriaDTOList();
            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _uow.CategoriaRepository.GetCategorias(categoriasParameters);
            return ObterCategorias(categorias);
        }

        

        [HttpGet("filter/nome/pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas(
            [FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categorias = _uow.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltro);

            return ObterCategorias(categorias);

        }
        
        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = categorias.ToCategriaDTOList();
            return Ok(categoriasDto);
        }
        
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _uow.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id {id} não encontrada...");
                return NotFound($"Categoria com id {id} não encontrada...");
            }

            var categoriaDto = categoria.ToCategoriaDTO();
            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uow.CategoriaRepository.Create(categoria);
            _uow.Commit();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId },
                novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var categoria = categoriaDto.ToCategoria();

           var categoriaAtualizada =  _uow.CategoriaRepository.Update(categoria);
            _uow.Commit();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();
            
            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uow.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id {id} não localizada...");
                return NotFound($"Categoria com id {id} não localizada...");
            }

            var categoriaExcluida = _uow.CategoriaRepository.Delete(categoria);
            _uow.Commit();

            var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();
            
            return Ok(categoriaExcluidaDto);
        }
    }
}