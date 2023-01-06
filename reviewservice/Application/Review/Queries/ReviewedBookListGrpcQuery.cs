using Ardalis.GuardClauses;
using AutoMapper;
using Consul;
using Grpc.Net.Client;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using reviewservice.Application.Review.Dtos;
using reviewservice.Domain.ReviewAggregate.Interfaces;
using reviewservice.Domain.ReviewAggregate.Specs;
using reviewservice.Protos;
using sharedkernel;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;
using sharedsecurity;
using System.Text;

namespace reviewservice.Application.Review.Queries
{
    public class ReviewedBookListGrpcQuery : IRequest<IServiceResponse<IList<ReviewBookDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Token { get; set; }

        public class ReviewedBookListGrpcQueryHandler : IRequestHandler<ReviewedBookListGrpcQuery, IServiceResponse<IList<ReviewBookDto>>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly ITokenService _tokenService;
            private readonly IMapper _mapper;           
            private readonly ConsulHostInfo _consulConfig;
            public ReviewedBookListGrpcQueryHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IOptions<ConsulHostInfo> consulConfig)
            {
                _unitOfWork = unitOfWork;
                _tokenService = tokenService;
                _mapper = mapper;                
                _consulConfig = consulConfig.Value;
            }

            public async Task<IServiceResponse<IList<ReviewBookDto>>> Handle(ReviewedBookListGrpcQuery request, CancellationToken cancellationToken)
            {
                var userId = this._tokenService.GetClaimValueFromToken(request.Token.Split(" ")[1], "UId");
                Guard.Against.NullOrEmpty(userId, message: "Claim is not valid");

                var spec = new ReviewedBooksSpec(request.Page, request.PageSize, Guid.Parse(userId));
                var result = await this._unitOfWork.ReviewRepository.FindAsync(spec).ConfigureAwait(false);

                var mappedList = this._mapper.Map<IList<ReviewBookDto>>(result);
                var idList = mappedList.Select(x => x.BookId).ToList();
                
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
            private async Task<IList<ReviewBookDetail>> GetBookDetails(string token, List<Guid> bookIdList, int page, int pageSize)
            {
                var bookServiceAddress = this.GetServiceAddress("BookCatalogService");

                if (bookServiceAddress == null)
                    return null;

                var url = bookServiceAddress.AbsoluteUri;

                using var channel = GrpcChannel.ForAddress(url);
                var client = new BookCatalogDetailService.BookCatalogDetailServiceClient(channel);

                var idList = bookIdList.Select(x => x.ToString()).ToList();
                var request = new BookDetailRequest()
                {
                    Page = page,
                    PageSize = pageSize,
                };
                request.BookIds.AddRange(idList);

                var result = client.GetReviewedBookDetails(request, null);

                List<ReviewBookDetail> response = new List<ReviewBookDetail>();
                
                while (await result.ResponseStream.MoveNext(default))
                {
                    var bookDetails = result.ResponseStream.Current.BookDetails;
                    foreach (var detail in bookDetails)
                    {
                        response.Add(new ReviewBookDetail()
                        {
                            Id = Guid.Parse(detail.Id),
                            FirstPublishedDate = detail.FirstPublishedDate,
                            Name = detail.Name,
                            Author = new ReviewBookAuthorDetail() { Id = Guid.Parse(detail.AuthorId), Name = detail.AuthorName },
                            Genre = new ReviewBookGenreDetail() { Id = detail.GenreId, Name = detail.GenreName }

                        });

                    }

                }

                return response;

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
                            return new Uri($"http://{serviceAddress.Value.Address}:8181");
                        }
                    }
                }

                return null;

            }
        }


    }
}
