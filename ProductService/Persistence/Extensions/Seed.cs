using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.DbContext;

namespace Persistence.Extensions
{
    public static class Seed
    {
        public static IHost SeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
              // var mongoContext = scope.ServiceProvider.GetRequiredService<MongoContext>();
             //  var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager>();

               var seed = scope.ServiceProvider.GetRequiredService<MongoContextSeed>();

               seed.SeedAsync().GetAwaiter().GetResult();

            }

            return host;
        }

    }
}