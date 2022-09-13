using System;
namespace bookcatalogservice.Application.Genre.Dtos
{
    public class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPopular { get; set; }
    }
}

