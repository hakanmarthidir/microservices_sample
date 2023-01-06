namespace bookcatalogservice.Application.Book.Dtos
{
    public class ReviewedBooksDetailDto
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public List<Guid> ReviewedBookIdLIst { get; set; }

    }
}

