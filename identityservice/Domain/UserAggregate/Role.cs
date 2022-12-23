using Ardalis.GuardClauses;
using sharedkernel;
using sharedkernel.Interfaces;

namespace identityservice.Domain.UserAggregate
{
    public class Role : BaseEntity<Guid>, ISoftDeletable, IAuditable
    {
        public string? Name { get; private set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public Status Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public bool IsDefault { get; set; }
        private readonly List<User> _users = new List<User>();
        public IReadOnlyCollection<User> Users => _users.AsReadOnly();

        public void Delete()
        {
            this.DeletedBy = Thread.CurrentPrincipal?.Identity?.Name ?? "Unknown";
            this.DeletedDate = DateTimeOffset.UtcNow;
            this.Status = Status.Deleted;
        }

        private Role() { }

        private Role(string roleName, bool isDefault) : this()
        {
            this.Id = Guid.NewGuid();
            this.Name = Guard.Against.NullOrWhiteSpace(roleName, nameof(roleName), "Role name could not be null.");
            this.CreatedDate = DateTimeOffset.UtcNow;
            this.CreatedBy = Thread.CurrentPrincipal?.Identity?.Name ?? "Unknown";
            this.IsDefault = isDefault;
        }

        public static Role CreateRole(string name, bool isDefault = false)
        {
            return new Domain.UserAggregate.Role(name, isDefault);
        }
    }
}

