using System;
using AutoMapper;
using bookcatalogservice.Application.Book.Dtos;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using bookcatalogservice.Domain.BookAggregate.Specs;
using MediatR;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;

namespace bookcatalogservice.Application.Book.Queries
{

    public class GetAllBooksQuery : IRequest<IServiceResponse<IList<BookDto>>>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IServiceResponse<IList<BookDto>>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public GetAllBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<IServiceResponse<IList<BookDto>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
            {
                BookSpec spec = new BookSpec(request.Page.GetValueOrDefault(1), request.PageSize.GetValueOrDefault(20));

                var result = await this._unitOfWork.BookRepository.FindAsync(spec).ConfigureAwait(false);

                var mappedResult = this._mapper.Map<IList<BookDto>>(result);

                return ServiceResponse<IList<BookDto>>.Success(mappedResult, "Book List");

            }
        }
    }
}

