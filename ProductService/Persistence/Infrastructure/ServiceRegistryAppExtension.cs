using System;
using Consul;
using Domain.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Persistence.Infrastructure
{
    public static class ServiceRegistryAppExtension
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, ConsulSettings consulSetting)
        {
            // var consulSetting = services.GetRequiredService<IConsulSettings>();
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(consulSetting.ConsulAddresss);
            }));
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
             var consulSetting = app.ApplicationServices.GetRequiredService<IConsulSettings>();
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            //var uri = new Uri(address);
            var registration = new AgentServiceRegistration()
            {
                ID = consulSetting.ServiceName, //{uri.Port}",
                // servie name  
                Name = consulSetting.ServiceName,
                Address = consulSetting.ServiceHost, //$"{uri.Host}",
                Port = consulSetting.ServicePort  // uri.Port
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from Consul");
            });

            return app;
        }
    }
}