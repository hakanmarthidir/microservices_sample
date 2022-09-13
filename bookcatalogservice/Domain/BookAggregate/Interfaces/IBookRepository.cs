using sharedkernel.Interfaces;

namespace bookcatalogservice.Domain.BookAggregate.Interfaces
{
    public interface IBookRepository : IQueryRepository<Book>, ICommandRepository<Book>
    {
    }
}

