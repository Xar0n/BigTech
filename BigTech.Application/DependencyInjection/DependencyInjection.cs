using BigTech.Application.Mapping;
using BigTech.Application.Services;
using BigTech.Application.Validations;
using BigTech.Application.Validations.FluentValidations.Report;
using BigTech.Domain.Dto.Report;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Interfaces.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BigTech.Application.DependencyInjection;
public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ReportMapping));
        services.AddAutoMapper(typeof(UserMapping));
        services.AddAutoMapper(typeof(RoleMapping));

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
