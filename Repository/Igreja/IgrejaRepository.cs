using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repository.Igreja;

public class IgrejaRepository : Repository<Models.Igreja>, IIgrejaRepository
{
    public IgrejaRepository(AppDbContext contexto) : base(contexto)
    {
    }

    public async Task<PagedList<Models.Igreja>> GetIgreja(IgrejaParameters produtosParameters)
    {
        //return Get()
        //        .OrderBy(on => on.Nome)
        //        .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
        //        .Take(produtosParameters.PageSize)
        //        .ToList();
        return await PagedList<Models.Igreja>.ToPagedList(Get().OrderBy(on => on.IgrejaId),
            produtosParameters.PageNumber, produtosParameters.PageSize);
    }
    //public async Task<IEnumerable<Models.Igreja>> GetProdutosPorPreco()
    //{
    //    return await Get().OrderBy(c => c.Preco).ToListAsync();
    //}
}
