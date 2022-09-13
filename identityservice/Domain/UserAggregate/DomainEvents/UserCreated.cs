using System;
using sharedkernel;

namespace identityservice.Domain.UserAggregate.DomainEvents
{
    public class UserCreated : DomainEvent
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public int RoleId { get; set; }

        public UserCreated(Guid id, string email, int roleId)
        {
            this.UserId = id;
            this.UserEmail = email;
            this.RoleId = roleId;
            this.CreatedDate = DateTimeOffset.UtcNow;
        }
    }
}

