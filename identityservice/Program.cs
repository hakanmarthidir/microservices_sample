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
using sharedsecurity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(identityservice.Application.Mappers.AutoMappings));
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var dbConnection = Environment.GetEnvironmentVariable("IDENTITY_DEFAULTCONNECTION");
builder.Services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(dbConnection));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IHashService, HashService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));

var consulConfig = builder.Configuration.GetSection("CONSUL").Get<ConsulServiceInfo>();
builder.Services.AddSingleton(consulConfig);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHealthChecks().AddSqlServer(dbConnection);

var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    AllowCachingResponses = false
});

IHostApplicationLifetime lifetime = app.Lifetime;
app.RegisterConsul(lifetime);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

