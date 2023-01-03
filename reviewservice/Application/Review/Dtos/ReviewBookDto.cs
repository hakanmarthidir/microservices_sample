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
    }
}
