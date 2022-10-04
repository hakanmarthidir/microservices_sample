using System;
using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using sharedkernel;

namespace bookcatalogservice.Infrastructure
{
    public static class ConsulClientRegister
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            var serviceInfo = app.ApplicationServices.GetRequiredService<ConsulServiceInfo>();
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceInfo.ConsulHost}:{serviceInfo.ConsulPort}"));

            //var httpCheck = new AgentServiceCheck()
            //{
            //    HTTP = $"http://{serviceInfo.IP}:{serviceInfo.Port}/healthz",
            //    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
            //    Timeout = TimeSpan.FromSeconds(10),
            //    Interval = TimeSpan.FromSeconds(30),
            //    Name = "BookCatalogHealthCheck"
            //};

            var registration = new AgentServiceRegistration()
            {
                //Checks = new[] { httpCheck },
                ID = $"{serviceInfo.ServiceName}-{Guid.NewGuid().ToString()}",
                Name = serviceInfo.ServiceName,
                Address = serviceInfo.ServiceIP,
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

