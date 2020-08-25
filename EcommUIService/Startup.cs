using EcommUIService.Extensions;
using EcommUIService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace EcommUIService
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
              var consulSettings = new ConsulSettings();  
            new ConfigureFromConfigurationOptions<ConsulSettings>(Configuration.GetSection("ConsulConfig")).Configure(consulSettings);  
           // services.AddSingleton(consulSettings);
            services.AddSingleton<IConsulSettings>(consulSettings);
            services.AddConsulConfig(consulSettings);
            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    //WithOrigins("http://localhost:4200");
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseConsul();
            //app.UseHttpsRedirection();
           
            app.UseSerilogRequestLogging();         
            app.UseRouting();
             app.UseCors("CorsPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                  endpoints.MapControllers();
                  endpoints.MapControllerRoute("spa-fallback", "{controller=Fallback}/action=Index");
            });
        }
    }
}
