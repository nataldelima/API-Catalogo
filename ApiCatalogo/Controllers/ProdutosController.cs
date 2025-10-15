using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ILogger _logger;

        public ProdutosController(IProdutoRepository produtoRepository, ILogger<ProdutosController> logger)
        {
            _produtoRepository = produtoRepository;
            _logger = logger;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _produtoRepository.GetProdutoCategoria(id);
            if (produtos is null)
            {
                _logger.LogWarning("Produtos não encontrados...");
                return NotFound("Produtos não encontrados...");
            }

            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _produtoRepository.GetAll();
            if (produtos is null)
            {
                _logger.LogWarning("Produtos não encontrados...");
                return NotFound("Produtos não encontrados...");
            }

            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _produtoRepository.Get(p => p.ProdutoId == id);
            if (produto is null)
            {
                _logger.LogWarning($"Produto com id {id} não encontrado...");
                return NotFound($"Produto com id {id} não encontrado...");
            }

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var novoProduto = _produtoRepository.Create(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning($"Produto com id = {id} não localizado ...");
                return BadRequest($"Produto com id = {id} não localizado ...");
            }

            var produtoAtualizado = _produtoRepository.Update(produto);
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _produtoRepository.Get(p => p.ProdutoId == id);
            if (produto is null)
            {
                _logger.LogWarning($"Produto com id = {id} não localizado ...");
                return BadRequest($"Produto com id = {id} não localizado ...");
            }

            var produtoDeletado = _produtoRepository.Delete(produto);
            return Ok($"Produto de id = {id} foi excluído");
        }
    }
}