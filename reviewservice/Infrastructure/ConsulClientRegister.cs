﻿using Consul;
using Microsoft.Extensions.Configuration;
using sharedkernel;

namespace reviewservice.Infrastructure
{
    public static class ConsulClientRegister
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration configuration)
        {
            var serviceInfo = configuration.GetSection("CONSULREVIEWSERVICEINFO").Get<ConsulReviewServiceInfo>();
            var hostInfo = configuration.GetSection("CONSULHOSTINFO").Get<ConsulHostInfo>();

            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{hostInfo.ConsulHost}:{hostInfo.ConsulPort}"));

            var httpCheck = new AgentServiceCheck()
            {
                HTTP = $"http://{serviceInfo.ServiceIp}:{serviceInfo.ServicePort}/healthz",
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20),
                Timeout = TimeSpan.FromSeconds(5),
                Interval = TimeSpan.FromSeconds(5),
                Name = "ReviewHealthCheck"
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = $"{serviceInfo.ServiceName}-{Guid.NewGuid().ToString()}",
                Name = serviceInfo.ServiceName,
                Address = serviceInfo.ServiceIp,
                Port = serviceInfo.ServicePort,
                Tags = new[] { "Review", "Book", "User" }
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

