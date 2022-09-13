using System.Collections.Generic;

namespace sharedkernel.Interfaces
{
    public interface IEntityDomainEvent
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void RaiseEvent(DomainEvent domainEvent);
        void RemoveEvents(DomainEvent domainEvent);
        void ClearEvents();
    }
}

