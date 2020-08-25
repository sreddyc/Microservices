// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using IdentityServer4.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Data;
using IdentityServer4.Services;
using IdentityServer.Services;
using Microsoft.Extensions.Options;
using IdentityServer.Extensions;
using AutoMapper;
using IdentityServer.Helpers;
using Serilog;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
         
        }

        public void ConfigureServices(IServiceCollection services)
        {
         // _logger.LogInfo("Inside configure services");

            var appSettings = new AppSettings();
            new ConfigureFromConfigurationOptions<AppSettings>(Configuration.GetSection("AppSettings")).Configure(appSettings);
            services.AddSingleton(appSettings);

            //  var identitySettings = new IdentitySettings();  
            //   new ConfigureFromConfigurationOptions<IIdentitySettings>(Configuration.GetSection("IdentitySettings")).Configure(identitySettings);  
            //    services.AddSingleton(identitySettings); 
            services.Configure<IdentitySettings>(
                   Configuration.GetSection(nameof(IdentitySettings)));
            services.AddSingleton<IIdentitySettings>(sp =>
               sp.GetRequiredService<IOptions<IdentitySettings>>().Value);

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfiles());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


            //  string connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            //services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton((Serilog.ILogger)Log.Logger);

            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(appSettings.DatabaseConnection, sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();


            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.UserInteraction.LoginUrl = "/Account/Login";
                options.UserInteraction.LogoutUrl = "/Account/Logout";
                options.Authentication = new AuthenticationOptions()
                {
                    CookieLifetime = TimeSpan.FromHours(10), // ID server cookie timeout set to 10 hours
                    CookieSlidingExpiration = true
                };
            });
            if (Environment.IsDevelopment())
            {
                var identitySettings = new IdentitySettings();
                new ConfigureFromConfigurationOptions<IdentitySettings>(Configuration.GetSection("IdentitySettings")).Configure(identitySettings);
                //services.AddSingleton(identitySettings);  
                var config = new Config(identitySettings, mapper);
                builder.AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(config.GetIdentityResources())
                .AddInMemoryApiResources(config.GetApiResources())
                .AddInMemoryClients(config.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                  .AddDeveloperSigningCredential();
            }
            else
            {
                services.AddDbContext<Data.ConfigurationDbContext>(options => options.UseSqlServer(appSettings.DatabaseConnection, sql => sql.MigrationsAssembly(migrationsAssembly)));

                builder.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(appSettings.DatabaseConnection, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(appSettings.DatabaseConnection, sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<ApplicationUser>();
                var cert = appSettings.CertificateFile.GetCertificate(appSettings.CertificateKey);
                builder.AddSigningCredential(cert);
                services.AddScoped<SeedData>();
                services.AddScoped<IProfileService, ProfileService>();
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            app.UseStaticFiles();

            // Add this line; you'll need `using Serilog;` up the top, too
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
