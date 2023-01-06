namespace reviewservice.Application.Review.Dtos
{
    public class ReviewCreateDto
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public byte Rating { get; set; }
        public string Comment { get; set; }
        public DateTime DateRead { get; set; }
    }
}
