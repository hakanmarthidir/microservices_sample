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
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public Guid AuthorId { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
