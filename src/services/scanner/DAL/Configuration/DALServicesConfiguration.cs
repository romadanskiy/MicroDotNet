using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Configuration;

public static class DALServicesConfiguration
{
    public static void ConfigureDALServices(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddDbContext<ScannerDbContext>(options =>
            options.UseNpgsql("Host=localhost; Port=5433;Database=scanner;Username=postgres;Password=postgres"));
    }
}