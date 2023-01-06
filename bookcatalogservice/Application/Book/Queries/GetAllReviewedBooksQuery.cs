using AutoMapper;
using bookcatalogservice.Application.Book.Dtos;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using bookcatalogservice.Domain.BookAggregate.Specs;
using MediatR;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;

namespace bookcatalogservice.Application.Book.Queries
{
    public class GetAllReviewedBooksQuery : IRequest<IServiceResponse<IList<BookDto>>>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public List<Guid> ReviewedBookIdList { get; set; }

        public class GetAllReviewedBooksQueryHandler : IRequestHandler<GetAllReviewedBooksQuery, IServiceResponse<IList<BookDto>>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public GetAllReviewedBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<IServiceResponse<IList<BookDto>>> Handle(GetAllReviewedBooksQuery request, CancellationToken cancellationToken)
            {
                BookReviewSpec spec = new BookReviewSpec(request.Page.GetValueOrDefault(1), request.PageSize.GetValueOrDefault(20), request.ReviewedBookIdList);

                var result = await this._unitOfWork.BookRepository.FindAsync(spec).ConfigureAwait(false);

                var mappedResult = this._mapper.Map<IList<BookDto>>(result);

                return ServiceResponse<IList<BookDto>>.Success(mappedResult, "Book List");

            }
        }
    }
}

