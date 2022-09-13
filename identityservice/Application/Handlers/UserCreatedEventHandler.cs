using System;
using identityservice.Domain.UserAggregate;
using identityservice.Domain.UserAggregate.DomainEvents;
using identityservice.Infrastructure.Security;
using MediatR;
using sharedkernel;

namespace identityservice.Application.Handlers
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreated>
    {

        private readonly ILogService<UserCreatedEventHandler> _logger;

        public UserCreatedEventHandler(ILogService<UserCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
        {
            //TODO: add logic
        }


    }


}

