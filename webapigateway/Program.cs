using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Consul;
using sharedsecurity;
using Ocelot.Provider.Polly;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Ocelot.Tracing.OpenTracing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle())
    .AddConsul()
    .AddPolly()
    //.AddOpenTracing()
    ;


var serviceName = "OcelotWebApiGateway";
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseOcelot().Wait();
app.Run();

