using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Extensions;
using IdentityServer.Services;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Diagnostics;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                // .WriteTo.RollingFile("Logs/{Date}.log")
                .WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/{Date}.ndjson")
                .CreateLogger();

            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().MigrateDatabase().Run();
                //return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                //return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                    .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);

#if DEBUG
                // Used to filter out potentially bad data due debugging.
                // Very useful when doing Seq dashboards and want to remove logs under debugging session.
                loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
            }) .ConfigureWebHostDefaults(webBuilder =>
                {
            webBuilder.UseStartup<Startup>();

        });
    }
}
