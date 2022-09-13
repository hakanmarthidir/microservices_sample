using identityservice.Domain.UserAggregate;
using identityservice.Domain.UserAggregate.Interfaces;
using sharedkernel;

namespace identityservice.Infrastructure.Persistence
{
    public class RoleRepository : RepositoryBase<IdentityContext, Domain.UserAggregate.Role>, IRoleRepository
    {
        public RoleRepository(IdentityContext dbContext) : base(dbContext)
        {

        }
    }
}

