using sharedkernel;

namespace bookcatalogservice.Domain.Aggregates.BookAggregate.Specs
{
    public class GetAllAuthorsSpec : BaseSpec<Author>
    {
        public GetAllAuthorsSpec(int page, int pageSize) : base()
        {
            AddPaging(page, pageSize);
        }
    }
}

