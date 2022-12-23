
using System;
using System.Text.Json.Serialization;
using Ardalis.GuardClauses;
using identityservice.Domain.UserAggregate.DomainEvents;
using identityservice.Domain.UserAggregate.ValueObjects;
using sharedkernel;
using sharedkernel.Interfaces;

namespace identityservice.Domain.UserAggregate
{
    public class User : BaseEntity<Guid>, IAggregateRoot, ISoftDeletable, IAuditable
    {
        public FullName FullName { get; private set; }
        public Email Email { get; private set; }
        [JsonIgnore]
        public Parole Password { get; private set; }
        public int RoleId { get; private set; }
        public Domain.UserAggregate.Role Role { get; set; }

        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public Status Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public UserActivation Activation { get; private set; }

        private readonly List<RefreshToken> _tokens = new List<RefreshToken>();
        public IReadOnlyCollection<RefreshToken> Tokens => _tokens.AsReadOnly();

        private User() { }

        private User(string name, string surname, string email, string password, int roleId) : this()
        {
            this.Id = Guid.NewGuid();
            this.RoleId = Guard.Against.NegativeOrZero(roleId, nameof(roleId), "Role could not be null.");
            this.FullName = FullName.CreateFullName(name, surname);
            this.Email = Email.CreateEmail(email);
            this.Password = Parole.CreateParole(password);
            this.CreatedDate = DateTimeOffset.UtcNow;
            this.Activation = UserActivation.CreateUserActivation();
            this.CreatedBy = Thread.CurrentPrincipal?.Identity?.Name ?? "Unknown";

            RaiseEvent(new UserCreated(this.Id, this.Email.EmailAddress, this.RoleId));
        }

        public static User CreateUser(string userName, string userSurname, string userEmail, string userPassword, int role)
        {
            var user = new User(userName, userSurname, userEmail, userPassword, role);
            return user;
        }

        public void Delete()
        {
            this.DeletedBy = Thread.CurrentPrincipal?.Identity?.Name ?? "Unknown";
            this.DeletedDate = DateTimeOffset.UtcNow;
            this.Status = Status.Deleted;
        }
    }
}

