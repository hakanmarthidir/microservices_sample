using System;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using MediatR;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;

namespace bookcatalogservice.Application.Genre.Commands
{
    public class CreateGenreCommand : IRequest<IServiceResponse>
    {
        public string Name { get; set; }
        public bool IsPopular { get; set; }

        public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, IServiceResponse>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateGenreCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IServiceResponse> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
            {
                var genre = Domain.BookAggregate.Genre.CreateGenre(request.Name, request.IsPopular);
                await this._unitOfWork.GenreRepository.InsertAsync(genre).ConfigureAwait(false);
                await this._unitOfWork.SaveAsync();
                return ServiceResponse.Success("Genre was created successfully.");
            }
        }
    }
}

