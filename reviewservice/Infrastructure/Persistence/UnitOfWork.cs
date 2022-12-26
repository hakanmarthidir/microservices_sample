
using reviewservice.Domain.ReviewAggregate.Interfaces;

namespace reviewservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ReviewContext _context;

        public UnitOfWork(ReviewContext context)
        {
            _context = context;
        }

        private IReviewRepository _reviewRepository { get; }      

        public IReviewRepository ReviewRepository { get { return _reviewRepository ?? new ReviewRepository(this._context); } }     

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

