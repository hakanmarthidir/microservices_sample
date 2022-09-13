using sharedkernel.Interfaces;

namespace bookcatalogservice.Domain.Aggregates.BookAggregate.Interfaces
{
    public interface IGenreRepository : IQueryRepository<Genre>, ICommandRepository<Genre>
    {
    }
}

