using sharedkernel.Interfaces;

namespace bookcatalogservice.Domain.Aggregates.BookAggregate.Interfaces
{
    public interface IBookRepository : IQueryRepository<Book>, ICommandRepository<Book>
    {
    }
}

