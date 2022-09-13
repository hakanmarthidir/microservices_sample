using Ardalis.GuardClauses;
using sharedkernel;

namespace identityservice.Domain.UserAggregate.ValueObjects
{
    public class Email : ValueObject
    {
        public string EmailAddress { get; private set; }

        private Email() { }

        public Email(string emailAddress) : this()
        {
            this.EmailAddress = Guard.Against.NullOrWhiteSpace(emailAddress, nameof(emailAddress), "Email could not be null");
        }

        public static Email CreateEmail(string email)
        {
            return new Email(email);
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailAddress;
        }
    }
}

