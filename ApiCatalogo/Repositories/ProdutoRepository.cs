using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos =  GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
        var produtosOrdenados =
            PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);
        return produtosOrdenados;
    }

    public IEnumerable<Produto> GetProdutoCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}