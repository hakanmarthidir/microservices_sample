using bookcatalogservice.Application.Author.Dtos;
using bookcatalogservice.Application.Genre.Dtos;

namespace bookcatalogservice.Application.Book.Dtos
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int FirstPublishedDate { get; set; }
        public AuthorDto Author { get; set; }
        public GenreDto Genre { get; set; }


    }
}

