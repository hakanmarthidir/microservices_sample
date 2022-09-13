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
            CreateMap<Domain.Aggregates.BookAggregate.Book, BookDto>();
            CreateMap<Domain.Aggregates.BookAggregate.Genre, GenreDto>();
            CreateMap<Domain.Aggregates.BookAggregate.Author, AuthorDto>();
        }
    }
}

