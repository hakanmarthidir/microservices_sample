using Ardalis.GuardClauses;
using AutoMapper;
using Azure.Core;
using MediatR;
using reviewservice.Application.Review.Dtos;
using reviewservice.Domain.ReviewAggregate.Interfaces;
using reviewservice.Domain.ReviewAggregate.Specs;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;
using sharedsecurity;

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

            public ReviewedBookListQueryHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _tokenService = tokenService;
                _mapper = mapper;
            }

            public async  Task<IServiceResponse<IList<ReviewBookDto>>> Handle(ReviewedBookListQuery request, CancellationToken cancellationToken)
            {
                var userId = this._tokenService.GetClaimValueFromToken(request.Token.Split(" ")[1], "UId");
                Guard.Against.NullOrEmpty(userId, message: "Claim is not valid");

                var spec = new ReviewedBooksSpec(request.Page, request.PageSize, Guid.Parse(userId));

                var result = await this._unitOfWork.ReviewRepository.FindAsync(spec).ConfigureAwait(false);

                var mappedList = this._mapper.Map<IList<ReviewBookDto>>(result);
                return ServiceResponse<IList<ReviewBookDto>>.Success(mappedList, "ReviewList");        
            }
       }
    }
}
