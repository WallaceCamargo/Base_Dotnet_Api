using APICatalago;
using APICatalago.Pagination;

namespace APICatalago.Repository.Ministerio;

public interface IMinisterioRepository : IRepository<Models.Ministerio>
{
    Task<PagedList<Models.Ministerio>> GetMinisterioPaginado(MinisterioParameters categoriasParameters);
    Task<IEnumerable<Models.Ministerio>> GetGetMinisterioIgrejas();
}
