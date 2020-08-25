using IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
//using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using System.Collections.Generic;
using AutoMapper;
using IdentityServer.Services;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Data
{
    public class SeedData
    {
        private readonly IdentityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ConfigurationDbContext _configDbContext;

        private readonly IServiceProvider _provider;

        private readonly IIdentitySettings _identitySettings;

        private readonly IMapper _mapper;

        private readonly ILogger<SeedData>  _logger;


        public SeedData(IdentityDbContext context,
                   UserManager<ApplicationUser> userManager,
                   ConfigurationDbContext configDbContext,
                   IServiceProvider provider,
                   IIdentitySettings identitySettings,
                   IMapper mapper,
                   ILogger<SeedData> logger
                   )
        {
            _context = context;
            _userManager = userManager;
            _configDbContext = configDbContext;
            _provider = provider;
            _identitySettings = identitySettings;
            _mapper = mapper;
            _logger = logger;
        }
        public void EnsureSeedData()
        {
            try
            {
                _logger.LogInformation("Seeding database...");
                Console.WriteLine("Seeding database...");
                PerformMigrations();

                EnsureSeedUserData().GetAwaiter().GetResult();
                Console.WriteLine("Done seeding user database.");

                SeedApiResource();
                SeedClients();
                SeedIdentityResource();

                Console.WriteLine("Done seeding config database.");
                _logger.LogInformation("Done seeding database...");
            }
            catch
            {
                _logger.LogError("Done seeding database...");
            }
        }

        private void PerformMigrations()
        {
            _provider.GetRequiredService<IdentityDbContext>().Database.Migrate();
            _provider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
            _provider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        }


        private async Task<bool> EnsureSeedUserData()
        {
            if (_context.Users.Any(r => r.UserName == "admin")) return false;
            //Create the default Admin account and apply the Administrator role
            string user = "admin";
            string password = "P@ssword1";
            await _userManager.CreateAsync(new ApplicationUser { UserName = user, Email = user, EmailConfirmed = true, IsEnabled = true }, password);
            return true;
        }

        private void SeedClients()
        {

            if (!_configDbContext.Clients.Any())
            {
                Console.WriteLine("Clients being populated");
                var clients = _mapper.Map<IEnumerable<Client>>(_identitySettings.Clients);
                foreach (var client in clients)
                {
                    _configDbContext.Clients.Add(client.ToEntity());
                }
                _configDbContext.SaveChanges();

            }
            else
            {
                Console.WriteLine("Clients already populated");
            }
        }

        private void SeedIdentityResource()
        {
            if (!_configDbContext.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");

                var identityResource = _mapper.Map<IEnumerable<IdentityResource>>(_identitySettings.IdentityResources);

                foreach (var identres in identityResource)
                {
                    _configDbContext.IdentityResources.Add(identres.ToEntity());
                }
                _configDbContext.SaveChanges();

            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }
        }

        private void SeedApiResource()
        {
            if (!_configDbContext.ApiResources.Any())
            {
                Console.WriteLine("ApiResources being populated");

                var apiResource = _mapper.Map<IEnumerable<ApiResource>>(_identitySettings.ApiResources);

                foreach (var apiRes in apiResource)
                {
                    _configDbContext.ApiResources.Add(apiRes.ToEntity());
                }
                _configDbContext.SaveChanges();

            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }
        }

    }
}
