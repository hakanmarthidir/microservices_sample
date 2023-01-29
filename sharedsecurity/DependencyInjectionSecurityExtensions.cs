using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace sharedsecurity
{
    public static class DependencyInjectionSecurityExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddTransient<IHashService, HashService>();
            services.AddTransient<ITokenService, TokenService>();

            var authenticationProviderKey = Environment.GetEnvironmentVariable("AUTHENTICATION_PROVIDERKEY");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = authenticationProviderKey;
                x.DefaultChallengeScheme = authenticationProviderKey;
            }).AddJwtBearer(authenticationProviderKey, x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWTCONFIG_ISSUER"),
                    ValidateAudience = true,
                    ValidAudience = Environment.GetEnvironmentVariable("JWTCONFIG_AUDIENCE"),
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTCONFIG_SECRET")))
                };
            });

            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy("AuthorizedClient", policy => policy.RequireClaim(ClaimTypes.Role, "Client"));
                    options.AddPolicy("AuthorizedAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
                    options.AddPolicy("Authorized", policy => policy.RequireClaim(ClaimTypes.Role, "Client", "Administrator"));
                }
                );

            return services;
        }

    }
}