using identityservice.Domain.UserAggregate.Interfaces;
using sharedkernel;

namespace identityservice.Infrastructure.Persistence
{
    public class RefreshTokenRepository : RepositoryBase<IdentityContext, Domain.UserAggregate.RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IdentityContext dbContext) : base(dbContext)
        {

        }
    }
}

