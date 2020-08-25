using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Product Write API", Version = "v1"});
                  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
              //  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
              //  c.IncludeXmlComments(xmlPath);

                // var securitySchema = new OpenApiSecurityScheme
                // {
                //     Description = "JWT Auth Bearer Scheme",
                //     Name = "Authorization",
                //     In = ParameterLocation.Header,
                //     Type = SecuritySchemeType.Http,
                //     Scheme = "bearer",
                //     Reference = new OpenApiReference
                //     {
                //         Type = ReferenceType.SecurityScheme,
                //         Id = "Bearer"
                //     }
                // };

                // c.AddSecurityDefinition("Bearer", securitySchema);
                // var securityRequirement = new OpenApiSecurityRequirement {{securitySchema, new[] {"Bearer"}}};
                // c.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumention(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => {c
                .SwaggerEndpoint("/swagger/v1/swagger.json", "Product write API v1");});

            return app;
        }
    }
}