using APICatalago.Models;
using APICatalago.Pagination;

namespace APICatalago.Repository.Igreja;

public interface IIgrejaRepository : IRepository<Models.Igreja>
{
    Task<PagedList<Models.Igreja>> GetIgreja(IgrejaParameters produtosParameters);
    //Task<IEnumerable<Models.Igreja>> GetProdutosPorPreco();
}
