using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using reviewservice.Application.Review.Dtos;
using reviewservice.Domain.ReviewAggregate.Interfaces;
using reviewservice.Domain.ReviewAggregate.Specs;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;
using sharedsecurity;
using System.Text;
using System.Text.Json;

namespace reviewservice.Application.Review.Queries
{
    public class ReviewedBookListQuery : IRequest<IServiceResponse<IList<ReviewBookDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Token { get; set; }

        public class ReviewedBookListQueryHandler : IRequestHandler<ReviewedBookListQuery, IServiceResponse<IList<ReviewBookDto>>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly ITokenService _tokenService;
            private readonly IMapper _mapper;
            private readonly IHttpClientFactory _httpClientFactory;
            public ReviewedBookListQueryHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IHttpClientFactory httpClientFactory)
            {
                _unitOfWork = unitOfWork;
                _tokenService = tokenService;
                _mapper = mapper;
                _httpClientFactory = httpClientFactory;
            }

            public async  Task<IServiceResponse<IList<ReviewBookDto>>> Handle(ReviewedBookListQuery request, CancellationToken cancellationToken)
            {
                var userId = this._tokenService.GetClaimValueFromToken(request.Token.Split(" ")[1], "UId");
                Guard.Against.NullOrEmpty(userId, message: "Claim is not valid");

                var spec = new ReviewedBooksSpec(request.Page, request.PageSize, Guid.Parse(userId));

                var result = await this._unitOfWork.ReviewRepository.FindAsync(spec).ConfigureAwait(false);

                foreach (var item in result)
                {
                    Console.WriteLine("Reviewed : " + item.UserId + " " + item.BookId);
                }

                var mappedList = this._mapper.Map<IList<ReviewBookDto>>(result);

                var idList = mappedList.Select(x => x.BookId).ToList();
                var detailResult = await this.GetBookDetails(request.Token,idList).ConfigureAwait(false);
                Console.WriteLine("Detail Count : " + detailResult.Count());
                return ServiceResponse<IList<ReviewBookDto>>.Success(mappedList, "ReviewList");        
            }            
            public async Task<IEnumerable<ReviewBookDetail>> GetBookDetails(string token, List<Guid> bookIdList)
            {
                IEnumerable<ReviewBookDetail>? bookDetails = null;
                var detailDto = new ReviewedBooksDetailDto() { ReviewedBookIdLIst = bookIdList };
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(detailDto), Encoding.UTF8, "application/json");

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://bookcatalogservice/api/v1/catalog/book/revieweddetails")
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                        { HeaderNames.Authorization, token}
                    }, 
                    Content = httpContent
                };

                var httpClient = _httpClientFactory.CreateClient();
                
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    bookDetails = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<ReviewBookDetail>>(contentStream);

                    foreach (var detail in bookDetails)
                    {
                        Console.WriteLine("Reviewed Book : " + detail.BookName);
                    }
                }

                return bookDetails;
            }

        }
    }
}
