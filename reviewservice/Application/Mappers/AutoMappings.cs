using System;
using AutoMapper;
using reviewservice.Application.Review.Dtos;

namespace reviewservice.Application.Mappers
{
    public class AutoMappings : AutoMapper.Profile
    {
        public AutoMappings()
        {
            CreateMap<Domain.ReviewAggregate.Review, ReviewBookDto>();            
        }
    }
}

