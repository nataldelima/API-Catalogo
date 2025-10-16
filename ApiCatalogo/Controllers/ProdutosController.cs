using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IUnitOfWork uow, ILogger<ProdutosController> logger, IMapper mapper)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
        {
            var produtos = _uow.ProdutoRepository.GetProdutoCategoria(id);
            if (produtos is null)
            {
                _logger.LogWarning("Produtos não encontrados...");
                return NotFound("Produtos não encontrados...");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uow.ProdutoRepository.GetAll();
            if (produtos is null)
            {
                _logger.LogWarning("Produtos não encontrados...");
                return NotFound("Produtos não encontrados...");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uow.ProdutoRepository.Get(p => p.ProdutoId == id);
            if (produto is null)
            {
                _logger.LogWarning($"Produto com id {id} não encontrado...");
                return NotFound($"Produto com id {id} não encontrado...");
            }

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return produtoDto;
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var produto = _mapper.Map<Produto>(produtoDto);
            var novoProduto = _uow.ProdutoRepository.Create(produto);
            _uow.Commit();

            var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                _logger.LogWarning($"Produto com id = {id} não localizado ...");
                return BadRequest($"Produto com id = {id} não localizado ...");
            }

            var produto = _mapper.Map<Produto>(produtoDto);
            var produtoAtualizado = _uow.ProdutoRepository.Update(produto);
            _uow.Commit();
            var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);
            return Ok(produtoAtualizadoDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uow.ProdutoRepository.Get(p => p.ProdutoId == id);
            if (produto is null)
            {
                _logger.LogWarning($"Produto com id = {id} não localizado ...");
                return BadRequest($"Produto com id = {id} não localizado ...");
            }

            var produtoDeletado = _uow.ProdutoRepository.Delete(produto);
            _uow.Commit();
            var produtoDeletadoDto = _mapper.Map<Produto>(produtoDeletado);
            return Ok($"Produto de id = {id} foi excluído");
        }
    }
}