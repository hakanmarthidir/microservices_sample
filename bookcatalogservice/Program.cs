using System.Reflection;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using bookcatalogservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using bookcatalogservice.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using sharedkernel;
using sharedsecurity;
using Prometheus;
using Prometheus.SystemMetrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.Configure<ConsulHostInfo>(builder.Configuration.GetSection("CONSULHOSTINFO"));
builder.Services.Configure<ConsulCatalogServiceInfo>(builder.Configuration.GetSection("CONSULCATALOGSERVICEINFO"));

var dbConnection = Environment.GetEnvironmentVariable("CATALOG_DEFAULTCONNECTION");
//var dbConnection = "Data Source=catalogsqlserver;Initial Catalog=bookservice;User Id=sa;Password=server2019!!;MultipleActiveResultSets=True;TrustServerCertificate=True";

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));
builder.Services.AddDbContext<BookContext>(options => options.UseSqlServer(dbConnection));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddSystemMetrics();

var serviceName = "BookCatalogService";
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
