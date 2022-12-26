using reviewservice.Domain.ReviewAggregate;
using sharedkernel.Interfaces;

namespace reviewservice.Domain.ReviewAggregate.Interfaces
{
    public interface IReviewRepository : IQueryRepository<Review>, ICommandRepository<Review>
    {
    }
}

