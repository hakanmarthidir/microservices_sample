using bookcatalogservice.Domain.BookAggregate;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using sharedkernel;

namespace bookcatalogservice.Infrastructure.Persistence
{
    public class BookRepository : RepositoryBase<BookContext, Book>, IBookRepository
    {
        public BookRepository(BookContext context) : base(context)
        {
        }
    }
}

