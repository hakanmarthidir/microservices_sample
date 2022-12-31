using MediatR;
using shelveservice.Domain.ShelveAggregate.Interfaces;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;

namespace shelveservice.Application.Shelve.Commands
{
    public class CreateShelveCommand : IRequest<IServiceResponse>
    {
        public Guid UserId { get; set; }       
        public string Name { get; set; }

        public class CreateReviewCommandHandler : IRequestHandler<CreateShelveCommand, IServiceResponse>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CreateReviewCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IServiceResponse> Handle(CreateShelveCommand request, CancellationToken cancellationToken)
            {
                
                await this._unitOfWork.ShelveRepository.InsertAsync(Domain.ShelveAggregate.Shelve.CreateShelve(request.UserId, request.Name), cancellationToken).ConfigureAwait(false);
                await this._unitOfWork.SaveAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse.Success("Shelve was created successfully.");
            }
        }

    }
}
