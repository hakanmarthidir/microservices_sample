using System;
using AutoMapper;
using bookcatalogservice.Application.Genre.Dtos;
using bookcatalogservice.Domai.BookAggregate.Specs;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using bookcatalogservice.Domain.BookAggregate.Specs;
using MediatR;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;

namespace bookcatalogservice.Application.Genre.Queries
{
    public class GetAllGenresQuery : IRequest<IServiceResponse<IList<GenreDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public class GetAllQueryHandler : IRequestHandler<GetAllGenresQuery, IServiceResponse<IList<GenreDto>>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;


            public GetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<IServiceResponse<IList<GenreDto>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
            {
                var spec = new GetAllGenresSpec(request.Page, request.PageSize);
                var result = await this._unitOfWork.GenreRepository.FindAsync(spec).ConfigureAwait(false);
                var mappedList = this._mapper.Map<IList<GenreDto>>(result);
                return ServiceResponse<IList<GenreDto>>.Success(mappedList, "GenreList");
            }
        }


    }
}

