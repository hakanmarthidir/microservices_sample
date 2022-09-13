using System;
using bookcatalogservice.Domain.Aggregates.BookAggregate;
using bookcatalogservice.Domain.Aggregates.BookAggregate.Interfaces;
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

