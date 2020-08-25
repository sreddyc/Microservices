using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Persistence.Services
{
    public class LoggerManager : ILoggerManager
    {
        private static Serilog.ILogger logger = Log.Logger;

        // public LoggerManager(IConfiguration configuration)
        // {
        //      Log.Logger = new LoggerConfiguration()
        //        .ReadFrom.Configuration(configuration)
        //             .Enrich.WithProperty("ApplicationName", typeof(LoggerManager).Assembly.GetName().Name)
        //                 // .Enrich.WithProperty("Environment", env.EnvironmentName)
        //          //.WriteTo.Providers(providers)
        //         .CreateLogger();
        //      logger = Log.Logger;
        // }
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