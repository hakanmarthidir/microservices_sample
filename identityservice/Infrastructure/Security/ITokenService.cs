using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace identityservice.Infrastructure.Security
{
    public interface ITokenService
    {
        string GenerateToken(Claim[] claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetTokenClaimPrincipal(string token);
    }
}

