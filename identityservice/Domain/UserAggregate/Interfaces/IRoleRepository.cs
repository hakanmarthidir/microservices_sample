using identityservice.Domain.UserAggregate;
using sharedkernel.Interfaces;

namespace identityservice.Domain.UserAggregate.Interfaces
{
    public interface IRoleRepository : ICommandRepository<Domain.UserAggregate.Role>, IQueryRepository<Domain.UserAggregate.Role>
    {
    }
}

