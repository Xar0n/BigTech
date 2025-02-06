using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace BigTech.Api;

public static class Startup
{
    /// <summary>
    /// Подключение Swagger
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddApiVersioning()
            .AddApiExplorer(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo() {
                Version = "v1",
                Title = "BigTech.Api",
                Description = "It is first version",
                Contact = new OpenApiContact()
                {
                    Email = "toni.neczvetaev.06@bk.ru",
                    Name = "Email"
                }
            });
            o.SwaggerDoc("v2", new OpenApiInfo()
            {
                Version = "v2",
                Title = "BigTech.Api",
                Description = "It is second version",
                Contact = new OpenApiContact()
                {
                    Email = "toni.neczvetaev.06@bk.ru",
                    Name = "Email"
                }
            });
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Введите токен",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme() {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
