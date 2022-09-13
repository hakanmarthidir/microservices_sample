using bookcatalogservice.Domain.BookAggregate.Interfaces;

namespace bookcatalogservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly BookContext _context;

        public UnitOfWork(BookContext context)
        {
            _context = context;
        }

        private IGenreRepository _genreRepository { get; }
        private IAuthorRepository _authorRepository { get; }
        private IBookRepository _bookRepository { get; }

        public IBookRepository BookRepository { get { return _bookRepository ?? new BookRepository(this._context); } }

        public IAuthorRepository AuthorRepository { get { return _authorRepository ?? new AuthorRepository(this._context); } }

        public IGenreRepository GenreRepository { get { return _genreRepository ?? new GenreRepository(this._context); } }

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

