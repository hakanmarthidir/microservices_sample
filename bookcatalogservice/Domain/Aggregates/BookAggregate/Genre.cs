using Ardalis.GuardClauses;
using sharedkernel;

namespace bookcatalogservice.Domain.Aggregates.BookAggregate
{
    public class Genre : BaseEntity<int>
    {
        public string Name { get; private set; }
        public bool IsPopular { get; private set; }
        public List<Book> Books { get; private set; }

        private Genre()
        { }

        private Genre(string name, bool isPopular) : this()
        {
            this.Name = Guard.Against.NullOrWhiteSpace(name, nameof(name), "Genre name could not be null.");
            this.IsPopular = isPopular;
        }

        public static Genre CreateGenre(string name, bool isPopular = false)
        {
            return new Genre(name, isPopular);
        }
    }

}

