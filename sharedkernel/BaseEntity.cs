using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using sharedkernel.Interfaces;

namespace sharedkernel
{
    public abstract class BaseEntity<TId> : IEntity, IEntityDomainEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TId? Id { get; set; }

        private List<DomainEvent> _domainEvents = new List<DomainEvent>();
        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void RaiseEvent(DomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<DomainEvent>();
            this._domainEvents.Add(domainEvent);
        }

        public void RemoveEvents(DomainEvent domainEvent)
        {
            this._domainEvents?.Remove(domainEvent);
        }

        public void ClearEvents()
        {
            this._domainEvents?.Clear();
        }
    }
}

