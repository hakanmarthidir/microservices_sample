using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace sharedsecurity
{
    public static class SecurityExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfig>(configuration.GetSection("JWTCONFIG")); 
            
            var jwtConfiguration = configuration.GetSection("JWTCONFIG").Get<JwtConfig>();
            services.AddSingleton(jwtConfiguration);

            if (jwtConfiguration != null)
            {
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
                        ValidIssuer = jwtConfiguration.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtConfiguration.Audience,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret))
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
            }
            return services;
        }

    }
}