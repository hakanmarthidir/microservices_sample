using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Prometheus.SystemMetrics;
using shelveservice.Domain.ShelveAggregate.Interfaces;
using shelveservice.Infrastructure;
using shelveservice.Infrastructure.Persistence;
using sharedkernel;
using sharedmonitoring;
using sharedmonitoring.Middlewares;
using sharedsecurity;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSerilogExtension();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.Configure<ConsulHostInfo>(builder.Configuration.GetSection("CONSULHOSTINFO"));
builder.Services.Configure<ConsulShelveServiceInfo>(builder.Configuration.GetSection("CONSULSHELVESERVICEINFO"));

var dbConnection = Environment.GetEnvironmentVariable("SHELVE_DEFAULTCONNECTION");
//var dbConnection = "Data Source=localhost;Initial Catalog=shelveservice;User Id=sa;Password=server2019;MultipleActiveResultSets=True;TrustServerCertificate=True";

builder.Services.AddScoped<IShelveRepository, ShelveRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));
builder.Services.AddDbContext<ShelveContext>(options => options.UseSqlServer(dbConnection));

builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddSystemMetrics();


builder.Services.AddHttpClient();
builder.Services.AddJaegerOpenTelemetryTracing("ShelveService", "0.0.1");


var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions { AllowCachingResponses = false });

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

