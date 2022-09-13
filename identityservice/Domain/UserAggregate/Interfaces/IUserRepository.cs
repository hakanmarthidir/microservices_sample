using identityservice.Domain.UserAggregate;
using sharedkernel.Interfaces;

namespace identityservice.Domain.UserAggregate.Interfaces
{
    public interface IUserRepository : ICommandRepository<User>, IQueryRepository<User>
    {
    }
}

