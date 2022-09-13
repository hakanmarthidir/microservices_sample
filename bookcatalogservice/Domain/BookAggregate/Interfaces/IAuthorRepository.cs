using System;
using sharedkernel.Interfaces;

namespace bookcatalogservice.Domain.BookAggregate.Interfaces
{
    public interface IAuthorRepository : IQueryRepository<Author>, ICommandRepository<Author>
    {
    }
}

