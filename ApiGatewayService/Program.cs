using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ApiGatewayService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder(args)
              .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var buildConfig = config.Build();
                    var ocelotConfigFile = buildConfig.GetValue<string>("OcelotConfig");

                    config.AddJsonFile(ocelotConfigFile,
                        optional: false,
                        reloadOnChange: true);
                }).UseSerilog((hostingContext, config) =>
                    config.ReadFrom.Configuration(hostingContext.Configuration)
                )
                // .ConfigureServices(services =>
                //     {
                //         services.AddOcelot()
                //             .AddConsul()
                //             // Store the configuration in consul
                //             .AddConfigStoredInConsul();
                //     })
                    .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseStartup<Startup>();
                                // webBuilder.Configure(app =>
                                // {
                                //     app.UseOcelot().Wait();
                                // });

                            });
    }
}
