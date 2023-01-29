using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace sharedmonitoring
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddJaegerOpenTelemetryTracing(this IServiceCollection services, string serviceName, string serviceVersion, string agentHost = "jaeger", int agentPort = 6831)
        {

            services.AddOpenTelemetry()
            .WithTracing(b =>
            b.AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
            })
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddSqlClientInstrumentation()
            .AddMassTransitInstrumentation()
            .AddJaegerExporter(options =>
            {
                options.AgentHost = agentHost;
                options.AgentPort = agentPort;

            }).AddSource(serviceName).SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion))
            )
              .StartWithHost();

            return services;
        }

        public static ILoggingBuilder AddSerilogExtension(this ILoggingBuilder logging)
        {
            logging.ClearProviders();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                //.WriteTo.LogstashHttp("http://logstashoutput:5044")
                .CreateLogger();
            return logging.AddSerilog(logger);
        }
    }
}