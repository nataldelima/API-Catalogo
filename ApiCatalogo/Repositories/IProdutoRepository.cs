using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
    Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
    Task<IEnumerable<Produto>> GetProdutoCategoriaAsync(int id);
    
    
}