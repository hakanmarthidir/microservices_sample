using identityservice.Domain.UserAggregate;
using sharedkernel.Interfaces;

namespace identityservice.Domain.UserAggregate.Interfaces
{
    public interface IRefreshTokenRepository : ICommandRepository<RefreshToken>, IQueryRepository<RefreshToken>
    {
    }
}

