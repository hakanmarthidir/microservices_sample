using reviewservice.Domain.ReviewAggregate.Interfaces;

namespace reviewservice.Domain.ReviewAggregate.Interfaces
{
    public interface IUnitOfWork
    {
        IReviewRepository ReviewRepository { get; }       
        Task<int> SaveAsync(CancellationToken token = default(CancellationToken));
    }
}

