using bookcatalogservice.Domain.BookAggregate;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using sharedkernel;

namespace bookcatalogservice.Infrastructure.Persistence
{
    public class AuthorRepository : RepositoryBase<BookContext, Author>, IAuthorRepository
    {
        public AuthorRepository(BookContext context) : base(context)
        {
        }
    }
}

