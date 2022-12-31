using System;
using sharedkernel;
using sharedkernel.Interfaces;

namespace shelveservice.Domain.ShelveAggregate
{
    public class Shelve : BaseEntity<Guid>, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public string Name { get; private set; }        

        private Shelve() { }
        private Shelve(Guid userId, string name): this()
        {
            this.Id = Guid.NewGuid();
            this.UserId= userId;
            this.Name = name;
        }

        public static Shelve CreateShelve(Guid userId, string name)
        {         
            return new Shelve(userId, name);
        }
    }

}

