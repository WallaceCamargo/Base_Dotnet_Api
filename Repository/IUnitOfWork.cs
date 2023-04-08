using APICatalago.Repository.Ministerio;
using APICatalago.Repository.Igreja;

namespace APICatalago.Repository;

public interface IUnitOfWork
{
    IIgrejaRepository IgrejaRepository { get; }
    IMinisterioRepository MinisterioRepository { get; }

    Task Commit();
}
