using Ardalis.GuardClauses;
using sharedkernel;

namespace identityservice.Domain.UserAggregate.ValueObjects
{
    public class FullName : ValueObject
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }

        private FullName() { }

        public FullName(string name, string surname) : this()
        {
            this.Name = Guard.Against.NullOrWhiteSpace(name, nameof(name), "Name could not be null.");
            this.Surname = Guard.Against.NullOrWhiteSpace(surname, nameof(surname), "Surname could not be null.");
        }

        public static FullName CreateFullName(string firstname, string lastname)
        {
            return new FullName(firstname, lastname);
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Surname;
        }
    }
}

