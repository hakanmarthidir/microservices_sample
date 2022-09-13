using System;
using sharedkernel.Interfaces;

namespace bookcatalogservice.Domain.Aggregates.BookAggregate.Interfaces
{
    public interface IAuthorRepository : IQueryRepository<Author>, ICommandRepository<Author>
    {
    }
}

