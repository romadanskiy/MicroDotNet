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
        
    }
}