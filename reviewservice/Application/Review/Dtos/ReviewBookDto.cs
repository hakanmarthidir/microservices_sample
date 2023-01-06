namespace reviewservice.Application.Review.Dtos
{
    public class ReviewBookDto
    {
        public Guid Id { get; set; }
        public DateTime DateRead { get; set; }
        public Guid UserId { get; private set; }
        public Guid BookId { get; private set; }
        public byte Rating { get; private set; }
        public string Comment { get; private set; }

        public ReviewBookDetail Detail { get; set; }
    }

    public class ReviewBookDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int FirstPublishedDate { get; set; }
        public ReviewBookGenreDetail Genre { get; set; }
        public ReviewBookAuthorDetail Author { get; set; }
    }

    public class ReviewBookGenreDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ReviewBookAuthorDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ReviewResponse
    {
        public int ErrorCode { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public IList<ReviewBookDetail> Data { get; set; }

    }
}
