using System.IdentityModel.Tokens.Jwt;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace sharedsecurity
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;

        public TokenService()
        {
            this._jwtConfig = new JwtConfig()
            {
                Audience = Environment.GetEnvironmentVariable("JWTCONFIG_AUDIENCE"),
                Issuer = Environment.GetEnvironmentVariable("JWTCONFIG_ISSUER"),
                Duration = double.Parse(Environment.GetEnvironmentVariable("JWTCONFIG_DURATION")),
                RefreshTokenDuration = double.Parse(Environment.GetEnvironmentVariable("JWTCONFIG_REFRESHTOKEN_DURATION")),
                Secret = Environment.GetEnvironmentVariable("JWTCONFIG_SECRET")
            };
        }

        public string GenerateToken(Claim[] claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken
                (
                claims: claims,
                issuer: this._jwtConfig.Issuer,
                audience: this._jwtConfig.Audience,
                expires: DateTime.Now.AddHours(this._jwtConfig.Duration.GetValueOrDefault(1)),
                signingCredentials: credentials,
                notBefore: DateTime.Now
                );

            var generatedToken = tokenHandler.WriteToken(token);
            Guard.Against.Null(generatedToken, "token", "Token could not be generated.");
            return generatedToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetTokenClaimPrincipal(string token)
        {
            Guard.Against.NullOrWhiteSpace(token, nameof(token), "Token could not be null.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));           

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = mySecurityKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = this._jwtConfig.Issuer,
                    ValidAudience = this._jwtConfig.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);


                if (!(validatedToken is JwtSecurityToken jwtSecurityToken)
                    || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetClaimValueFromToken(string token, string claimType)
        {
            var claimDetail = this.GetTokenClaimPrincipal(token);
            Guard.Against.Null(claimDetail, "token is not valid.");

            var selectedClaim = claimDetail.Claims.FirstOrDefault(x => x.Type == claimType);
            Guard.Against.Null(selectedClaim, "claim is not valid.");

            return selectedClaim.Value;

        }
    }
}

