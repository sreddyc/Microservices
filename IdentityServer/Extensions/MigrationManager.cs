using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace IdentityServer.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {

            using (var scope = host.Services.CreateScope())
            {
                IWebHostEnvironment env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                if (!env.IsDevelopment())
                {
                    var seed = scope.ServiceProvider.GetRequiredService<SeedData>();

                    seed.EnsureSeedData();
                }

            }

            return host;
        }


        public static async Task<bool> IsPkceClientAsync(this IClientStore store, string client_id)
        {
            if (!string.IsNullOrWhiteSpace(client_id))
            {
                var client = await store.FindEnabledClientByIdAsync(client_id);
                return client?.RequirePkce == true;
            }

            return false;
        }


    }
}
