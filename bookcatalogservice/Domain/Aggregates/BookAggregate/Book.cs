using System;
using Ardalis.GuardClauses;
using sharedkernel;
using sharedkernel.Interfaces;

namespace bookcatalogservice.Domain.Aggregates.BookAggregate
{
    public class Book : BaseEntity<Guid>, IAggregateRoot
    {
        public string Name { get; private set; }
        public int FirstPublishedDate { get; private set; }

        public Guid AuthorId { get; private set; }
        public Author Author { get; private set; }

        public int GenreId { get; private set; }
        public Genre Genre { get; private set; }

        private Book()
        {
        }

        private Book(string name, int firstPublishedYear, Guid authorId, int genreId) : this()
        {
            this.Name = Guard.Against.NullOrWhiteSpace(name, nameof(name), "Book name could not be null.");
            this.FirstPublishedDate = Guard.Against.NegativeOrZero(firstPublishedYear, nameof(firstPublishedYear), "Book year is invalid.");
            this.AuthorId = authorId;
            this.GenreId = genreId;

        }

        public static Book CreateBook(string name, int firstPublishedYear, Guid authorId, int genreId)
        {
            return new Book(name, firstPublishedYear, authorId, genreId);
        }
    }

}

