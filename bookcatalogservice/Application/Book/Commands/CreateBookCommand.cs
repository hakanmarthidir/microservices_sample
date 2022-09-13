using System;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using MediatR;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;

namespace bookcatalogservice.Application.Book.Commands
{
    public class CreateBookCommand : IRequest<IServiceResponse>
    {
        public string Name { get; set; }
        public int FirstPublishedDate { get; set; }
        public Guid AuthorId { get; set; }
        public int GenreId { get; set; }

        public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, IServiceResponse>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CreateBookCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IServiceResponse> Handle(CreateBookCommand request, CancellationToken cancellationToken)
            {
                var book = Domain.BookAggregate.Book.CreateBook(request.Name, request.FirstPublishedDate, request.AuthorId, request.GenreId);
                await this._unitOfWork.BookRepository.InsertAsync(book).ConfigureAwait(false);
                await this._unitOfWork.SaveAsync();

                return ServiceResponse.Success("Book was created successfully.");
            }
        }

    }
}

