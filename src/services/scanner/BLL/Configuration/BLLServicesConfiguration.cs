using BLL.Models;
using BLL.Models.Helpers;
using BLL.Services;
using DAL;
using DAL.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Configuration;

public static class BLLServicesConfiguration
{
    public static void ConfigureBLLServices(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.ConfigureDALServices();
        services.AddScoped<AccountService>();
        services.AddScoped<GarbageService>();
        services.AddScoped<ReceptionPointService>();
        services.AddSingleton<PasswordHelper>();
        services.AddSingleton<ImageHelper>();
    }
}