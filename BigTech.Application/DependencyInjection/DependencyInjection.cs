using BigTech.Application.Mapping;
using BigTech.Application.Services;
using BigTech.Application.Validations;
using BigTech.Application.Validations.FluentValidations.Report;
using BigTech.Domain.Dto.Report;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Interfaces.Validations;
using BigTech.Domain.Settings;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BigTech.Application.DependencyInjection;
public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ReportMapping));
        services.AddAutoMapper(typeof(UserMapping));
        services.AddAutoMapper(typeof(RoleMapping));

        var options = configuration.GetSection(nameof(RedisSettings));
        var redisUrl = options["Url"];
        var instanceName = options["InstanceName"];
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisUrl;
            options.InstanceName = instanceName;
        });

        InitServices(services);
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IReportValidator, ReportValidator>();
        services.AddScoped<IValidator<CreateReportDto>, CreateReportValidator>();
        services.AddScoped<IValidator<UpdateReportDto>, UpdateReportValidator>();

        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
    }
}
