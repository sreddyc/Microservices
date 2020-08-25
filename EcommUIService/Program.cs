using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EcommUIService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                    .UseSerilog((hostingContext, config) =>
                    config.ReadFrom.Configuration(hostingContext.Configuration)
                   .Enrich.FromLogContext()
                   .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                   .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
