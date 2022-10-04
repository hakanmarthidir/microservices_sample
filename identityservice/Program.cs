using identityservice.Application.Services;
using identityservice.Domain.UserAggregate;
using identityservice.Domain.UserAggregate.Interfaces;
using identityservice.Domain;
using identityservice.Infrastructure.Persistence;
using identityservice.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using identityservice;
using Consul;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using identityservice.Infrastructure;
using sharedkernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddAutoMapper(typeof(identityservice.Application.Mappers.AutoMappings));
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

builder.Services.AddTransient<IHashService, HashService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));

var consulConfig = builder.Configuration.GetSection("ConsulConfig").Get<ConsulServiceInfo>();
var jwtTokenConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
builder.Services.AddSingleton(jwtTokenConfig);
builder.Services.AddSingleton(consulConfig);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var authenticationProviderKey = "OcelotGuardKey";
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = authenticationProviderKey;
    x.DefaultChallengeScheme = authenticationProviderKey;
}).AddJwtBearer(authenticationProviderKey, x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtTokenConfig.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtTokenConfig.Audience,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.Secret))
    };
});


//ClaimBased Authorization
builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy("AuthorizedClient", policy => policy.RequireClaim(ClaimTypes.Role, "Client"));
    }
    );

builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    AllowCachingResponses = false
});

IHostApplicationLifetime lifetime = app.Lifetime;
app.RegisterConsul(lifetime);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

