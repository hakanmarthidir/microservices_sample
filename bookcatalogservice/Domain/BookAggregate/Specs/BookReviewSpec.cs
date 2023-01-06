using sharedkernel;

namespace bookcatalogservice.Domain.BookAggregate.Specs
{
    public class BookReviewSpec : BaseSpec<Book>
    {
        public BookReviewSpec(int page, int size, List<Guid> bookids) : base(x => bookids.Contains(x.Id) == true)
        {
            AddInclude(x => x.Author);
            AddInclude(x => x.Genre);
            AddPaging(page, size);
        }
    }
}

