using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Prometheus.SystemMetrics;
using reviewservice.Domain.ReviewAggregate.Interfaces;
using reviewservice.Infrastructure;
using reviewservice.Infrastructure.Persistence;
using reviewservice.Protos;
using sharedkernel;
using sharedmonitoring;
using sharedmonitoring.Middlewares;
using sharedsecurity;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// add Vault implementation (AddVault() extension is within sharedconfiguration)
//it will retrieve the related secrets and bind to environment variables for related docker container. 
// await builder.Configuration.AddVault();

builder.WebHost.UseKestrel(option =>
{
    option.ListenAnyIP(80, config =>
    {
        config.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2AndHttp3;
    });   
});

builder.Logging.AddSerilogExtension();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.Configure<ConsulHostInfo>(builder.Configuration.GetSection("CONSULHOSTINFO"));
builder.Services.Configure<ConsulReviewServiceInfo>(builder.Configuration.GetSection("CONSULREVIEWSERVICEINFO"));

var dbConnection = Environment.GetEnvironmentVariable("REVIEW_DEFAULTCONNECTION");
//var dbConnection = "Data Source=localhost;Initial Catalog=reviewservice;User Id=sa;Password=server2019;MultipleActiveResultSets=True;TrustServerCertificate=True";

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));
builder.Services.AddDbContext<ReviewContext>(options => options.UseSqlServer(dbConnection));

builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddSystemMetrics();

builder.Services.AddHttpClient();
builder.Services.AddJaegerOpenTelemetryTracing("ReviewService", "0.0.1");

builder.Services.AddGrpcClient<BookCatalogDetailService.BookCatalogDetailServiceClient>();

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

