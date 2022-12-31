
using shelveservice.Domain.ShelveAggregate.Interfaces;

namespace shelveservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ShelveContext _context;

        public UnitOfWork(ShelveContext context)
        {
            _context = context;
        }

        private IShelveRepository _shelveRepository { get; }      

        public IShelveRepository ShelveRepository { get { return _shelveRepository ?? new ShelveRepository(this._context); } }     

        public async Task<int> SaveAsync(CancellationToken token = default)
        {
            try
            {
                return await this._context.SaveChangesAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }
    }
}

