using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace IdentityServer.Services
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger; // =  new LoggerConfiguration();
        /*
                .ReadFrom.Configuration(Configuration)
                 .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                //.WriteTo.File(@"identityserver4_log.txt")
                 .WriteTo.RollingFile("Logs/{Date}.log")

                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
                */
        public LoggerManager(IConfiguration configuration)
        {
            try
            {
                if(logger == null)
                {
                logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
                }
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogInfo(string message)
        {
            logger.Information(message);
        }

        public void LogWarn(string message)
        {
            logger.Warning(message);
        }
    }
}
