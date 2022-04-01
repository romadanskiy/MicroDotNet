using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace RuOverflow.Questions.Infrastructure.ApplicationBuilderExtensions;

public static class ApplicationBuilderExtensions
{
    public static void UpdateDb<TContext>(this IApplicationBuilder app) where TContext : DbContext
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var contextFactory = serviceScope.ServiceProvider.GetService<IDbContextFactory<TContext>>();
        var context = contextFactory!.CreateDbContext();
        context.Database.Migrate();
    }
}
