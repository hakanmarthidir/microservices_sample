using System.Reflection;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using bookcatalogservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using bookcatalogservice.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using sharedkernel;
using sharedsecurity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
var dbConnection = Environment.GetEnvironmentVariable("CATALOG_DEFAULTCONNECTION");

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));

builder.Services.AddDbContext<BookContext>(options => options.UseSqlServer(dbConnection));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var consulConfig = builder.Configuration.GetSection("CONSULHOSTINFO").Get<ConsulHostInfo>();
builder.Services.AddSingleton(consulConfig);
var consulServiceConfig = builder.Configuration.GetSection("CONSULCATALOG").Get<ConsulServiceInfo>();
builder.Services.AddSingleton(consulServiceConfig);

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHealthChecks().AddSqlServer(dbConnection);

var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions{AllowCachingResponses = false});

IHostApplicationLifetime lifetime = app.Lifetime;
app.RegisterConsul(lifetime);

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    DatabaseManagementService.MigrationInitialize(app);
}


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
