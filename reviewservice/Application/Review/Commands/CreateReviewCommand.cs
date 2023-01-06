using Ardalis.GuardClauses;
using MediatR;
using reviewservice.Domain.ReviewAggregate.Interfaces;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;
using sharedsecurity;

namespace reviewservice.Application.Review.Commands
{
    public class CreateReviewCommand : IRequest<IServiceResponse>
    {
        public string Token { get; set; }
        public Guid BookId { get; set; }
        public byte Rating { get; set; }
        public string Comment { get; set; }
        public DateTime DateRead { get; set; }

        public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, IServiceResponse>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITokenService _tokenService;
            public CreateReviewCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
            {
                _unitOfWork = unitOfWork;
                _tokenService = tokenService;
            }

            public async Task<IServiceResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
            {
                var userId = this._tokenService.GetClaimValueFromToken(request.Token.Split(" ")[1], "UId");
                Guard.Against.NullOrEmpty(userId, message: "Claim is not valid");

                await this._unitOfWork.ReviewRepository.InsertAsync(Domain.ReviewAggregate.Review.CreateReview(Guid.Parse(userId), request.BookId, request.Rating, request.Comment, request.DateRead), cancellationToken).ConfigureAwait(false);
                await this._unitOfWork.SaveAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse.Success("Review was created successfully.");
            }
        }

    }
}
