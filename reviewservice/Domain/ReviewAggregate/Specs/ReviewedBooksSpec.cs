using sharedkernel;
using sharedkernel.Interfaces;

namespace reviewservice.Domain.ReviewAggregate.Specs
{
    public class ReviewedBooksSpec : BaseSpec<Review>
    {
        public ReviewedBooksSpec(int page, int pageSize, Guid userId) : base(x=> x.UserId == userId)
        {
            AddPaging(page, pageSize);
        }
    }    
}
