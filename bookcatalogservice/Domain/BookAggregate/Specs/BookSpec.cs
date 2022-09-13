using System;
using sharedkernel;

namespace bookcatalogservice.Domain.BookAggregate.Specs
{
    public class BookSpec : BaseSpec<Book>
    {
        public BookSpec(int page, int size) : base()
        {
            AddInclude(x => x.Author);
            AddInclude(x => x.Genre);
            AddPaging(page, size);
        }
    }
}

