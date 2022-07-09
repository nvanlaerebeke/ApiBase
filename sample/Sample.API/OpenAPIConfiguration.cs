using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Sample.API
{
    internal static class OpenAPIConfiguration
    {
        public static void Setup(IServiceCollection services) => services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme
            {
                Description = "Api key token validation",
                Name = "X-API-KEY",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "X-API-KEY"
                            },
                            Scheme = "X-API-KEY",
                            Name = "X-API-KEY",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
        });
    }
}
