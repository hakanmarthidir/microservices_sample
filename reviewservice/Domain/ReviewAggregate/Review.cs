using Ardalis.GuardClauses;
using reviewservice.Extensions;
using sharedkernel;

namespace reviewservice.Domain.ReviewAggregate
{
    public class Review : BaseEntity<Guid>
    {
        public Guid UserId { get; private set; }
        public Guid BookId { get; private set; }
        public DateTime? DateRead { get; private set; }
        public byte Rating { get; private set; }
        public string Comment { get; private set; }

        private Review() { }

        private Review(Guid UserId, Guid BookId, byte Rating, string Comment, DateTime DateRead) : this()
        {
            this.Id= Guid.NewGuid();
            this.UserId = Guard.Against.NullOrEmpty(UserId, "UserId could not be null");
            this.BookId = Guard.Against.NullOrEmpty(BookId, "BookId could not be null");
            this.Rating = Guard.Against.ByteNegative(Rating, "Rating could not be less than zero");
            this.Comment = Comment;
            this.DateRead = DateRead;
        }

        public static Review CreateReview(Guid UserId, Guid BookId, byte Rating, string Comment, DateTime DateRead)
        {
            return new Review(UserId, BookId, Rating, Comment, DateRead);
        }
    }

}

