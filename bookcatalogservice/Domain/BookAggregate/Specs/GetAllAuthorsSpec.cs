using sharedkernel;

namespace bookcatalogservice.Domain.BookAggregate.Specs
{
    public class GetAllAuthorsSpec : BaseSpec<Author>
    {
        public GetAllAuthorsSpec(int page, int pageSize) : base()
        {
            AddPaging(page, pageSize);
        }
    }
}

