using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace sharedmonitoring
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddJaegerOpenTelemetryTracing(this IServiceCollection services, string serviceName, string serviceVersion, string agentHost="jaeger", int agentPort=6831)
        {
            return services.AddOpenTelemetryTracing(b =>
            {
                b
                 .AddAspNetCoreInstrumentation(options =>
                 {
                     options.RecordException = true;
                 })
                .AddHttpClientInstrumentation()
                .AddSqlClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()                
                .AddJaegerExporter(options =>
                {
                    options.AgentHost = agentHost;
                    options.AgentPort = agentPort;

                })
                .AddSource(serviceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion));
            });
        }
    }
}