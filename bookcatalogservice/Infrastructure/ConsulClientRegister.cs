using System;
using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using sharedkernel;

namespace bookcatalogservice.Infrastructure
{
    public static class ConsulClientRegister
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration configuration)
        {
            var serviceInfo = configuration.GetSection("CONSULCATALOGSERVICEINFO").Get<ConsulCatalogServiceInfo>();
            var hostInfo = configuration.GetSection("CONSULHOSTINFO").Get<ConsulHostInfo>();

            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{hostInfo.ConsulHost}:{hostInfo.ConsulPort}"));

            var httpCheck = new AgentServiceCheck()
            {
                HTTP = $"http://{serviceInfo.ServiceIp}:{serviceInfo.ServicePort}/healthz",
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20),
                Timeout = TimeSpan.FromSeconds(5),
                Interval = TimeSpan.FromSeconds(5),
                Name = "BookCatalogHealthCheck"
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = $"{serviceInfo.ServiceName}-{Guid.NewGuid().ToString()}",
                Name = serviceInfo.ServiceName,
                Address = serviceInfo.ServiceIp,
                Port = serviceInfo.ServicePort,
                Tags = new[] { "Book", "Genre", "Author" }
            };

            consulClient.Agent.ServiceRegister(registration).Wait();
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}

