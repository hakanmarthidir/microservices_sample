using sharedkernel.Interfaces;

namespace bookcatalogservice.Domain.BookAggregate.Interfaces
{
    public interface IGenreRepository : IQueryRepository<Genre>, ICommandRepository<Genre>
    {
    }
}

