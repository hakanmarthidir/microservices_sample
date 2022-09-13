namespace bookcatalogservice.Domain.BookAggregate.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        IGenreRepository GenreRepository { get; }
        Task<int> SaveAsync(CancellationToken token = default(CancellationToken));
    }
}

