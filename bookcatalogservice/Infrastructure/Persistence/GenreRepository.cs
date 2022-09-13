using System;
using bookcatalogservice.Domain.BookAggregate;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using sharedkernel;

namespace bookcatalogservice.Infrastructure.Persistence
{
    public class GenreRepository : RepositoryBase<BookContext, Genre>, IGenreRepository
    {
        public GenreRepository(BookContext context) : base(context)
        {
        }
    }
}

