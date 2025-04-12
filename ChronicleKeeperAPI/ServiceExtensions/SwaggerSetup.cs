using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ChronicleKeeper.API.ServiceExtensions
{
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                //options.SwaggerDoc("v1", new OpenApiInfo { Title = "ChronicleKeeper API", Version = "v1" });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ChronicleKeeper API",
                    Version = "v1",
                    Description = "API for managing characters and lore in ChronicleKeeper.",
                    Contact = new OpenApiContact
                    {
                        Name = "Your Name",
                        Email = "your.email@example.com",
                        Url = new Uri("https://yourwebsite.com")
                    }
                });

                // ✅ Add JWT Authentication to Swagger UI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer {your token here}'"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
