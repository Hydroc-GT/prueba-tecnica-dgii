using DGII.Application.Interfaces;
using DGII.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DGII.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IContribuyenteService, ContribuyenteService>();
        services.AddScoped<IComprobanteFiscalService, ComprobanteFiscalService>();

        return services;
    }
}
