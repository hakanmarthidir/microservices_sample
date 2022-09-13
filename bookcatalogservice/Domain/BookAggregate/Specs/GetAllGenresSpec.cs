using sharedkernel;

namespace bookcatalogservice.Domai.BookAggregate.Specs
{
    public class GetAllGenresSpec : BaseSpec<Domain.BookAggregate.Genre>
    {
        public GetAllGenresSpec(int page, int pageSize) : base()
        {
            AddPaging(page, pageSize);
        }
    }
}

