using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
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

