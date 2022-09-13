namespace bookcatalogservice.Application.Book.Dtos
{
    public class BookCreateDto
    {
        public string BookName { get; set; }
        public int FirstPublishedYear { get; set; }
        public Guid AuthorId { get; set; }
        public int GenreId { get; set; }
    }
}

