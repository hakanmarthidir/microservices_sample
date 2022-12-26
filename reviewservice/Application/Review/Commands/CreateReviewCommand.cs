using MediatR;
using reviewservice.Domain.ReviewAggregate.Interfaces;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;

namespace reviewservice.Application.Review.Commands
{
    public class CreateReviewCommand : IRequest<IServiceResponse>
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public byte Rating { get; set; }
        public string Comment { get; set; }

        public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, IServiceResponse>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CreateReviewCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IServiceResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
            {
                
                await this._unitOfWork.ReviewRepository.InsertAsync(Domain.ReviewAggregate.Review.CreateReview(request.UserId, request.BookId, request.Rating, request.Comment), cancellationToken).ConfigureAwait(false);
                await this._unitOfWork.SaveAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse.Success("Review was created successfully.");
            }
        }

    }
}
