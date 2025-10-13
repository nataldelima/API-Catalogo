using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories;

public interface IProdutoRepository
{
    IQueryable<Produto> GetProdutos();
    Produto GetProduto(int id);
    Produto Create(Produto produto);
    bool Update(Produto produto);
    bool Delete(int id);
}