using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.Extensions;
using Serilog;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().SeedDatabase().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var buildConfig = config.Build();
                    var secretsFile= buildConfig.GetValue<string>("SecretsFile");
                    
                    config.AddJsonFile(secretsFile,
                        optional: true,
                        reloadOnChange: true);
                }).UseSerilog((hostingContext, config) =>
                    config.ReadFrom.Configuration(hostingContext.Configuration)
                   .Enrich.FromLogContext()
                   .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                   .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName)
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
