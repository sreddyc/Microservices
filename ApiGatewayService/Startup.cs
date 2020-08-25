using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Kubernetes;
using Microsoft.AspNetCore.Mvc;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Validator;
using Ocelot.Responses;

namespace ApiGatewayService
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
            services.AddOcelot()
              .AddConsul()
              .AddConfigStoredInConsul();
             // .AddKubernetes();
            // services.AddCors();
            services.AddCors(opt =>
             {
                 opt.AddPolicy("CorsPolicy", policy =>
                 {
                     policy.AllowAnyHeader().AllowAnyMethod()
                     .AllowAnyOrigin();
                    //.WithOrigins("http://localhost:4200")
                    //.AllowCredentials();
                });
             });
            services.AddApiVersioning(x =>
                {
                    x.DefaultApiVersion = new ApiVersion(1, 0);
                    x.AssumeDefaultVersionWhenUnspecified = true;
                    x.ReportApiVersions = true;
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
            //   app.UseSerilogRequestLogging(); 
            //  app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            // global cors policy
            // app.UseCors(x => x
            //     .AllowAnyMethod()
            //     .AllowAnyHeader()
            //     .SetIsOriginAllowed(origin => true) // allow any origin
            //     .AllowCredentials()); // allow credentials

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseOcelot().Wait();

        }
    }
}
