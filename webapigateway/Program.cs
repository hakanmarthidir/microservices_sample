using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Consul;
using sharedsecurity;
using Ocelot.Provider.Polly;
using sharedmonitoring;
using sharedmonitoring.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilogExtension();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddJwtAuthentication();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle())
    .AddConsul()
    .AddPolly();

builder.Services.AddHttpClient();
builder.Services.AddJaegerOpenTelemetryTracing("OcelotWebApiGateway", "0.0.1");

var app = builder.Build();

app.UseRouting();

app.UseCors(x => x
   .AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader());
           
app.UseCorrelationMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseOcelot().Wait();
app.Run();

