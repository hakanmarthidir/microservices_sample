using sharedkernel;

namespace bookcatalogservice.Domain.Aggregates.BookAggregate.Specs
{
    public class GetAllGenresSpec : BaseSpec<Domain.Aggregates.BookAggregate.Genre>
    {
        public GetAllGenresSpec(int page, int pageSize) : base()
        {
            AddPaging(page, pageSize);
        }
    }
}

