using identityservice.Domain.UserAggregate;
using identityservice.Domain.UserAggregate.Interfaces;
using sharedkernel;

namespace identityservice.Infrastructure.Persistence
{
    public class UserRepository : RepositoryBase<IdentityContext, Domain.UserAggregate.User>, IUserRepository
    {
        public UserRepository(IdentityContext dbContext) : base(dbContext)
        {

        }
    }
}

