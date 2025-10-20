using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);

    PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams);
    IEnumerable<Produto> GetProdutoCategoria(int id);
    
    
}