using identityservice.Domain.UserAggregate;
using sharedkernel;

namespace identityservice.Domain.UserAggregate.Specs
{
    public class GetRefreshTokenByUserSpec : BaseSpec<RefreshToken>
    {
        public GetRefreshTokenByUserSpec(Guid userId, string refreshToken) : base(x => x.UserId == userId
        && x.Token == refreshToken
        && x.ExpiresAt == DateTimeOffset.UtcNow)
        {

        }
    }
}

