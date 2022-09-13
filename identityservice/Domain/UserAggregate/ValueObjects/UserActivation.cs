using Ardalis.GuardClauses;
using identityservice.Domain.UserAggregate.Enums;
using sharedkernel;

namespace identityservice.Domain.UserAggregate.ValueObjects
{
    public class UserActivation : ValueObject
    {
        public ActivationStatusEnum IsActivated { get; private set; }
        public string ActivationCode { get; private set; }
        public DateTimeOffset? ActivationDate { get; private set; }

        private UserActivation()
        {
        }


        private UserActivation(ActivationStatusEnum activationStatusEnum, string activationCode, DateTimeOffset? activationDate) : this()
        {
            IsActivated = activationStatusEnum;
            ActivationCode = activationCode;
            ActivationDate = activationDate;
        }

        public static UserActivation CreateUserActivation()
        {
            var newUserActivation = new UserActivation(ActivationStatusEnum.NotActivated, Guid.NewGuid().ToString(), null);

            return newUserActivation;
        }

        public static UserActivation ActivateUser(string activationCode)
        {
            Guard.Against.NullOrWhiteSpace(activationCode, nameof(activationCode), "ActivationCode could not be null.");
            var activatedModel = new UserActivation(ActivationStatusEnum.Activated, activationCode, DateTimeOffset.UtcNow);

            return activatedModel;

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ActivationCode;
            yield return IsActivated;
        }
    }
}

