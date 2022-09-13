using System;
using AutoMapper;
using bookcatalogservice.Application.Author.Dtos;
using bookcatalogservice.Application.Book.Dtos;
using bookcatalogservice.Application.Genre.Dtos;

namespace bookcatalogservice.Application.Mappers
{
    public class AutoMappings : AutoMapper.Profile
    {
        public AutoMappings()
        {
            CreateMap<Domain.BookAggregate.Book, BookDto>();
            CreateMap<Domain.BookAggregate.Genre, GenreDto>();
            CreateMap<Domain.BookAggregate.Author, AuthorDto>();
        }
    }
}

