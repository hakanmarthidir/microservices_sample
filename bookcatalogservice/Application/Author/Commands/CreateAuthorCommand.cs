using System;
using bookcatalogservice.Domain.Aggregates.BookAggregate.Interfaces;
using MediatR;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;

namespace bookcatalogservice.Application.Author.Commands
{
    public class CreateAuthorCommand : IRequest<IServiceResponse>
    {
        public string Name { get; set; }
        public bool HasNobel { get; set; }

        public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, IServiceResponse>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateAuthorCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IServiceResponse> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
            {
                var author = Domain.Aggregates.BookAggregate.Author.CreateAuthor(request.Name, request.HasNobel);

                await this._unitOfWork.AuthorRepository.InsertAsync(author).ConfigureAwait(false);
                await this._unitOfWork.SaveAsync().ConfigureAwait(false);

                return ServiceResponse.Success("Author was created successfully.");
            }
        }
    }
}

