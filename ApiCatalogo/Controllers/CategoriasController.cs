using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _uow.CategoriaRepository.GetAllAsync();
            if (categorias is null)
            {
                _logger.LogWarning("Não existem categorias...");
                return NotFound("Não existem categorias...");
            }

            var categoriasDto = categorias.ToCategriaDTOList();
            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _uow.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
            return ObterCategorias(categorias);
        }

        

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas(
            [FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categorias = await _uow.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltro);

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
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _uow.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id {id} não encontrada...");
                return NotFound($"Categoria com id {id} não encontrada...");
            }

            var categoriaDto = categoria.ToCategoriaDTO();
            return Ok(categoriaDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uow.CategoriaRepository.Create(categoria);
            await _uow.CommitAsync();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId },
                novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var categoria = categoriaDto.ToCategoria();

           var categoriaAtualizada =  _uow.CategoriaRepository.Update(categoria);
            await _uow.CommitAsync();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();
            
            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uow.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id {id} não localizada...");
                return NotFound($"Categoria com id {id} não localizada...");
            }

            var categoriaExcluida = _uow.CategoriaRepository.Delete(categoria);
            await _uow.CommitAsync();

            var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();
            
            return Ok(categoriaExcluidaDto);
        }
    }
}