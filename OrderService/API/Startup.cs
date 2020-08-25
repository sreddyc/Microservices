using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Core.Entities.Common;
using Infrastructure.Extensions;
using Microsoft.Extensions.Options;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
              services.AddDbContext<StoreContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(x => 
            {
                x.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });
             services.AddAutoMapper(typeof(MappingProfiles));
            
             var consulSettings = new ConsulSettings();  
            new ConfigureFromConfigurationOptions<ConsulSettings>(Configuration.GetSection("ConsulConfig")).Configure(consulSettings);  
           // services.AddSingleton(consulSettings);
            services.AddSingleton<IConsulSettings>(consulSettings);
            services.AddConsulConfig(consulSettings);

            services.AddControllers();

            services.AddSingleton<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(Configuration
                    .GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddApplicationServices();
            services.AddIdentityServices(Configuration);
            services.AddSwaggerDocumentation();
            //services.AddCors();
            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    //WithOrigins("http://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
              app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            // app.UseCors(
            //     options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            // );
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerDocumention();
              app.UseConsul();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
