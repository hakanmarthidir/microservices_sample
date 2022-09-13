using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using identityservice.Domain.UserAggregate;
using sharedkernel;

namespace identityservice.Domain.UserAggregate
{
    public class RefreshToken : BaseEntity<Guid>
    {
        public string Token { get; private set; }
        public User User { get; private set; }
        public Guid UserId { get; private set; }
        public DateTimeOffset IssuedAt { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }

        private RefreshToken()
        {
        }

        public RefreshToken(string token, Guid userId, DateTimeOffset expiresAt) : this()
        {
            this.Token = Guard.Against.NullOrWhiteSpace(token, nameof(token), "Token could not be null.");
            this.UserId = Guard.Against.Null<Guid>(userId, nameof(userId), "User could not be null");
            this.IssuedAt = DateTimeOffset.UtcNow;
            this.ExpiresAt = expiresAt;
        }

        public static RefreshToken CreateRefreshToken(string token, Guid userId, DateTimeOffset expiresAt)
        {
            return new RefreshToken(token, userId, expiresAt);
        }

    }
}

