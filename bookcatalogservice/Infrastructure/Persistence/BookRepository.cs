using bookcatalogservice.Domain.Aggregates.BookAggregate;
using bookcatalogservice.Domain.Aggregates.BookAggregate.Interfaces;
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

