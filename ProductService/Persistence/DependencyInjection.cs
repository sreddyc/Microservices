using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence.DbContext;
using Persistence.Services;
using Serilog;
using Serilog.Extensions.Logging;
using MessagePublish.Sender;
using Persistence.Infrastructure;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.Configure<MongoSettings>(
               configuration.GetSection(nameof(MongoSettings)));
            services.AddSingleton<IMongoSettings>(sp =>
               sp.GetRequiredService<IOptions<MongoSettings>>().Value);

            services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));  

            services.AddAutoMapper(typeof(MappingProfiles));

            //  services.Configure<ConsulSettings>(
            //    configuration.GetSection(nameof(ConsulSettings)));
            // services.AddSingleton<IConsulSettings>(sp =>
            //    sp.GetRequiredService<IOptions<ConsulSettings>>().Value);

             var consulSettings = new ConsulSettings();  
            new ConfigureFromConfigurationOptions<ConsulSettings>(configuration.GetSection("ConsulConfig")).Configure(consulSettings);  
           // services.AddSingleton(consulSettings);
            services.AddSingleton<IConsulSettings>(consulSettings);
            services.AddConsulConfig(consulSettings);
            // Creating a `LoggerProviderCollection` lets Serilog optionally write
            // events through other dynamically-added MEL ILoggerProviders.
            // var providers = new LoggerProviderCollection();

            // Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //         .Enrich.WithProperty("ApplicationName", typeof(DependencyInjection).Assembly.GetName().Name)
            //     // .Enrich.WithProperty("Environment", env.EnvironmentName)
            //     .WriteTo.Providers(providers)
            //     .CreateLogger();


            // services.AddSingleton(providers);
            // services.AddSingleton<ILoggerFactory>(sc =>
            // {
            //     var providerCollection = sc.GetService<LoggerProviderCollection>();
            //     var factory = new SerilogLoggerFactory(null, true, providerCollection);

            //     foreach (var provider in sc.GetServices<ILoggerProvider>())
            //         factory.AddProvider(provider);

            //     return factory;
            // });

            // services.AddLogging();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IProductBrandRepository, ProductBrandRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<MongoContextSeed>();
            services.AddScoped<MongoContext>();
            services.AddTransient<IProductUpdateSender, ProductUpdateSender>();
         

            return services;
        }
    }
}