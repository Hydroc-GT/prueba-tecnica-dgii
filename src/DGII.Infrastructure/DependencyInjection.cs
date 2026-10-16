using DGII.Domain.Interfaces;
using DGII.Infrastructure.Context;
using DGII.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DGII.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
                               ?? "Data Source=dgii.db";

        services.AddDbContext<DgiiDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IContribuyenteRepository, ContribuyenteRepository>();
        services.AddScoped<IComprobanteFiscalRepository, ComprobanteFiscalRepository>();

        return services;
    }
}
