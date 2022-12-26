using reviewservice.Domain.ReviewAggregate;
using reviewservice.Domain.ReviewAggregate.Interfaces;
using reviewservice.Infrastructure.Persistence;
using sharedkernel;

namespace reviewservice.Infrastructure.Persistence
{
    public class ReviewRepository : RepositoryBase<ReviewContext, Review>, IReviewRepository
    {
        public ReviewRepository(ReviewContext context) : base(context)
        {
        }
    }
}

