using MediatR;

namespace sharedkernel
{
    public abstract class DomainEvent : INotification
    {
        public DateTimeOffset CreatedDate { get; protected set; } = DateTimeOffset.UtcNow;
    }
}

