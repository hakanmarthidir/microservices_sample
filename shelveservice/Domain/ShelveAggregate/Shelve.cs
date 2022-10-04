using System;
using sharedkernel;
using sharedkernel.Interfaces;

namespace shelveservice.Domain.ShelveAggregate
{
    public class Shelve : BaseEntity<Guid>, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public string Name { get; private set; }

        public List<ShelvedBook> ShelvedBooks { get; private set; }
    }

}

