using shelveservice.Domain.ShelveAggregate.Interfaces;

namespace shelveservice.Domain.ShelveAggregate.Interfaces
{
    public interface IUnitOfWork
    {
        IShelveRepository ShelveRepository { get; }       
        Task<int> SaveAsync(CancellationToken token = default(CancellationToken));
    }
}

