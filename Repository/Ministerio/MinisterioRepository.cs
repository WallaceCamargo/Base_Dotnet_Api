using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repository.Ministerio;

public class MinisterioRepository : Repository<Models.Ministerio>, IMinisterioRepository
{
    public MinisterioRepository(AppDbContext contexto) : base(contexto)
    {

    }

    public async Task<PagedList<Models.Ministerio>> GetMinisterioPaginado(MinisterioParameters MinisterioParameters)
    {
        return await PagedList<Models.Ministerio>.ToPagedList(Get().OrderBy(on => on.MinisterioId),
            MinisterioParameters.PageNumber, MinisterioParameters.PageSize);
    }

    public async Task<IEnumerable<Models.Ministerio>> GetGetMinisterioIgrejas()
    {
        return await Get().Include(x => x.Igrejas).ToListAsync();
    }

}
