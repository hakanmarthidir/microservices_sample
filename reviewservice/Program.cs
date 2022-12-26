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
using sharedkernel;
using sharedsecurity;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.Configure<ConsulHostInfo>(builder.Configuration.GetSection("CONSULHOSTINFO"));
builder.Services.Configure<ConsulReviewServiceInfo>(builder.Configuration.GetSection("CONSULREVIEWSERVICEINFO"));

var dbConnection = Environment.GetEnvironmentVariable("REVIEW_DEFAULTCONNECTION");
//var dbConnection = "Data Source=reviewsqlserver;Initial Catalog=reviewservice;User Id=sa;Password=server2019!!;MultipleActiveResultSets=True;TrustServerCertificate=True";

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));
builder.Services.AddDbContext<ReviewContext>(options => options.UseSqlServer(dbConnection));

builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddSystemMetrics();



var serviceName = "ReviewService";
var serviceVersion = "0.0.1";
builder.Services.AddHttpClient();

builder.Services.AddOpenTelemetryTracing(b =>
{
    b
     .AddAspNetCoreInstrumentation(options =>
     {
         options.RecordException = true;
     })
    .AddHttpClientInstrumentation()
    .AddSqlClientInstrumentation()
    .AddEntityFrameworkCoreInstrumentation()
    //.AddConsoleExporter()
    .AddJaegerExporter(options =>
    {
        var agentHost = "jaeger";
        var agentPort = 6831;
        options.AgentHost = agentHost;
        options.AgentPort = agentPort;

    })
    .AddSource(serviceName)
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion));
});


var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions { AllowCachingResponses = false });

app.RegisterConsul(app.Lifetime, builder.Configuration);

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    DatabaseManagementService.MigrationInitialize(app);
}


app.UseHttpMetrics();
app.UseAuthentication();
app.UseAuthorization();
app.MapMetrics();
app.MapControllers();
app.Run();

