using Ardalis.GuardClauses;
using sharedkernel;

namespace bookcatalogservice.Domain.BookAggregate
{
    public class Author : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public bool HasNobel { get; private set; }
        public List<Book> Books { get; private set; }

        private Author()
        { }

        private Author(string name, bool hasNobel = false) : this()
        {
            this.Name = Guard.Against.NullOrWhiteSpace(name, nameof(name), "Author name could not be null.");
            this.HasNobel = HasNobel;
        }

        public static Author CreateAuthor(string name, bool hasNobel = false)
        {
            return new Author(name, hasNobel);
        }
    }

}

