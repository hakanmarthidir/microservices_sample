using identityservice.Domain.UserAggregate;
using sharedkernel.Interfaces;

namespace identityservice.Domain.UserAggregate.Interfaces
{
    public interface IRoleRepository : ICommandRepository<Role>, IQueryRepository<Role>
    {
    }
}

