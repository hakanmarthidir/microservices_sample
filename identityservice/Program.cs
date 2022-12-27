using identityservice.Application.Services;
using identityservice.Domain.UserAggregate.Interfaces;
using identityservice.Infrastructure.Persistence;
using identityservice.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using identityservice.Infrastructure;
using sharedkernel;
using sharedsecurity;
using Prometheus;
using Prometheus.SystemMetrics;
using sharedmonitoring;
using sharedmonitoring.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSerilogExtension();
builder.Services.AddAutoMapper(typeof(identityservice.Application.Mappers.AutoMappings));
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.Configure<ConsulHostInfo>(builder.Configuration.GetSection("CONSULHOSTINFO"));
builder.Services.Configure<ConsulIdentityServiceInfo>(builder.Configuration.GetSection("CONSULIDENTITYSERVICEINFO"));


var dbConnection = Environment.GetEnvironmentVariable("IDENTITY_DEFAULTCONNECTION");
//var dbConnection = "Data Source=localhost;Initial Catalog=identityservice;User Id=sa;Password=server2019;MultipleActiveResultSets=True;TrustServerCertificate=True";

builder.Services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(dbConnection));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddSystemMetrics();



builder.Services.AddHttpClient();
builder.Services.AddJaegerOpenTelemetryTracing("IdentityService", "0.0.1");

var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions{ AllowCachingResponses = false});

app.RegisterConsul(app.Lifetime, builder.Configuration);

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    DatabaseManagementService.MigrationInitialize(app);
}

app.UseRouting();
app.UseCors(x => x
   .AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader());

app.UseCorrelationMiddleware();
app.UseHttpMetrics();
app.UseAuthentication();
app.UseAuthorization();
app.MapMetrics();
app.MapControllers();

app.Run();

