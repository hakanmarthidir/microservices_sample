using Grpc.Core;
using bookcatalogservice.Protos;
using AutoMapper;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using bookcatalogservice.Application.Book.Dtos;
using bookcatalogservice.Domain.BookAggregate.Specs;

namespace bookcatalogservice.GrpcServices
{
    public class GrpcBookService : BookCatalogDetailService.BookCatalogDetailServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GrpcBookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public override async Task GetReviewedBookDetails(BookDetailRequest request, IServerStreamWriter<BookList> responseStream, ServerCallContext context)
        {
            BookReviewSpec spec = new BookReviewSpec(request.Page, request.PageSize, request.BookIds.Select(x => Guid.Parse(x)).ToList());
            var result = await this._unitOfWork.BookRepository.FindAsync(spec).ConfigureAwait(false);
            var mappedResult = this._mapper.Map<IList<BookDto>>(result);                    

            var list = new BookList();
            var details = new Google.Protobuf.Collections.RepeatedField<BookDetailResponse>();

            foreach (var item in mappedResult)
            {
                list.BookDetails.Add(new BookDetailResponse()
                {
                    AuthorId = item.Author.Id.ToString(),
                    AuthorName = item.Author.Name,
                    GenreId = item.Genre.Id,
                    GenreName = item.Genre.Name,
                    FirstPublishedDate = item.FirstPublishedDate,
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            await responseStream.WriteAsync(list);
        }
    }
}
