namespace reviewservice.Application.Review.Queries
{
    public class ReviewedBooksDetailDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<Guid> ReviewedBookIdList { get; set; }
    }
}
