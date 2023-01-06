using Ardalis.GuardClauses;
using AutoMapper;
using Consul;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using reviewservice.Application.Review.Dtos;
using reviewservice.Domain.ReviewAggregate.Interfaces;
using reviewservice.Domain.ReviewAggregate.Specs;
using sharedkernel;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;
using sharedsecurity;
using System.Text;

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
            private readonly ConsulHostInfo _consulConfig;
            public ReviewedBookListQueryHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IHttpClientFactory httpClientFactory, IOptions<ConsulHostInfo> consulConfig)
            {
                _unitOfWork = unitOfWork;
                _tokenService = tokenService;
                _mapper = mapper;
                _httpClientFactory = httpClientFactory;
                _consulConfig = consulConfig.Value;
            }

            public async Task<IServiceResponse<IList<ReviewBookDto>>> Handle(ReviewedBookListQuery request, CancellationToken cancellationToken)
            {
                var userId = this._tokenService.GetClaimValueFromToken(request.Token.Split(" ")[1], "UId");
                Guard.Against.NullOrEmpty(userId, message: "Claim is not valid");

                var spec = new ReviewedBooksSpec(request.Page, request.PageSize, Guid.Parse(userId));
                var result = await this._unitOfWork.ReviewRepository.FindAsync(spec).ConfigureAwait(false);

                var mappedList = this._mapper.Map<IList<ReviewBookDto>>(result);
                var idList = mappedList.Select(x => x.BookId).ToList();

                //getreviewedbookdetails from another microservice by using REST
                var detailResult = await this.GetBookDetails(request.Token, idList, request.Page, request.PageSize).ConfigureAwait(false);
                if (detailResult != null)
                {
                    foreach (var item in mappedList)
                    {
                        item.Detail = detailResult.FirstOrDefault(x => x.Id == item.BookId);
                    }
                }

                return ServiceResponse<IList<ReviewBookDto>>.Success(mappedList, "ReviewList");
            }
            public async Task<IList<ReviewBookDetail>> GetBookDetails(string token, List<Guid> bookIdList, int page, int pageSize)
            {
                var bookServiceAddress = this.GetServiceAddress("BookCatalogService");

                if (bookServiceAddress == null)
                    return null;

                var detailDto = new ReviewedBooksDetailDto() { ReviewedBookIdList = bookIdList, Page = page, PageSize = pageSize };

                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(detailDto), Encoding.UTF8, "application/json");

                var url = $"http://{bookServiceAddress.AbsoluteUri}/api/v1/catalog/book/reviewed";

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
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
                    var reviewResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ReviewResponse>();
                    return reviewResponse.Data;
                }
                return null;

            }
            private Uri GetServiceAddress(string serviceName)
            {
                var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{_consulConfig.ConsulHost}:{_consulConfig.ConsulPort}"));
                var services = consulClient.Agent.Services().Result.Response;

                if (services.Any())
                {
                    foreach (var serviceAddress in services)
                    {
                        if (string.Equals(serviceAddress.Value.Service, serviceName, StringComparison.OrdinalIgnoreCase))
                        {
                            return new Uri($"{serviceAddress.Value.Address}:{serviceAddress.Value.Port}");
                        }
                    }
                }

                return null;

            }
        }


    }
}
