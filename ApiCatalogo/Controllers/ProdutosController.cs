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
        private readonly IProdutoRepository _repository;
        private readonly ILogger _logger;

        public ProdutosController(IProdutoRepository repository, ILogger<ProdutosController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetProdutos().ToList();
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
            var produto = _repository.GetProduto(id);
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

            var novoProduto = _repository.Create(produto);

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

            return _repository.Update(produto)
                ? Ok(produto)
                : StatusCode(500, $"Falha ao atualizar o produto de id = {id}!");
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _repository.GetProduto(id);
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning($"Produto com id = {id} não localizado ...");
                return BadRequest($"Produto com id = {id} não localizado ...");
            }

            
            return _repository.Delete(id)
                ? Ok($"Produto de id = {id} foi excluído")
                : StatusCode(500, $"Falha ao excluirr o produto de id = {id}!");
        }
    }
}