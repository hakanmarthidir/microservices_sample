using Ardalis.GuardClauses;
using sharedkernel;

namespace identityservice.Domain.UserAggregate.ValueObjects
{
    public class Parole : ValueObject
    {
        public string Password { get; private set; }


        private Parole() { }

        public Parole(string password) : this()
        {
            Guard.Against.NullOrWhiteSpace(password, nameof(password), "Password could not be null.");
            this.HashPassword(password);
        }

        public static Parole CreateParole(string password)
        {
            return new Parole(password);
        }

        private void HashPassword(string password)
        {
            //TODO: hash password
            this.Password = password;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Password;
        }
    }
}

