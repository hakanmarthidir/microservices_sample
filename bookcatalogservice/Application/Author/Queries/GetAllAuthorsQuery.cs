using System;
using AutoMapper;
using bookcatalogservice.Application.Author.Dtos;
using bookcatalogservice.Domain.Aggregates.BookAggregate.Interfaces;
using bookcatalogservice.Domain.Aggregates.BookAggregate.Specs;
using MediatR;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;

namespace bookcatalogservice.Application.Author.Queries
{
    public class GetAllAuthorsQuery : IRequest<IServiceResponse<IList<AuthorDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, IServiceResponse<IList<AuthorDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public GetAllAuthorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<IServiceResponse<IList<AuthorDto>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
            {
                var spec = new GetAllAuthorsSpec(request.Page, request.PageSize);
                var result = await this._unitOfWork.AuthorRepository.FindAsync(spec).ConfigureAwait(false);
                var mappedResult = this._mapper.Map<IList<AuthorDto>>(result);

                return ServiceResponse<IList<AuthorDto>>.Success(mappedResult, "AuthorList");

            }
        }
    }
}

