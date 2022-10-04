using System.Reflection;
using bookcatalogservice.Domain.BookAggregate.Interfaces;
using bookcatalogservice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using bookcatalogservice.Infrastructure;
using bookcatalogservice;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Consul;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using sharedkernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Add services to the container.
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));

builder.Services.AddDbContext<BookContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var consulConfig = builder.Configuration.GetSection("ConsulConfig").Get<ConsulServiceInfo>();
var jwtTokenConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
builder.Services.AddSingleton(jwtTokenConfig);
builder.Services.AddSingleton(consulConfig);

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

builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy("AuthorizedClient", policy => policy.RequireClaim(ClaimTypes.Role, "Client"));
        options.AddPolicy("AuthorizedAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
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
