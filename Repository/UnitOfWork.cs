using APICatalago.Context;
using APICatalago.Repository.Ministerio;
using APICatalago.Repository.Igreja;

namespace APICatalago.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IgrejaRepository _IgrejaRepo;
        private MinisterioRepository _MinisterioRepo;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext contexto)
        {
            _context= contexto;
        }

        public IIgrejaRepository IgrejaRepository
        {
            get { return _IgrejaRepo = _IgrejaRepo ?? new IgrejaRepository(_context); }
        }

        public IMinisterioRepository MinisterioRepository
        {
            get { return _MinisterioRepo = _MinisterioRepo ?? new MinisterioRepository(_context); }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
