using BigTech.DAL.Repositories;
using BigTech.Domain.Entity;
using BigTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BigTech.DAL.DependencyInjection;
public static class DependenctInjection
{
    public static void AddDataAcessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresSQL");
        
        services.InitRepositories();
        services.AddDbContext<ApplicationDbContext>(o =>
        {
            o.UseNpgsql(connectionString);
        });
    }

    private static void InitRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
        services.AddScoped<IBaseRepository<Report>, BaseRepository<Report>>();
    }
}
