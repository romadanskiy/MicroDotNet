using System;
using AuthorizationServer.Web.Data;
using AuthorizationServer.Web.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

namespace AuthorizationServer.Web;

public static class StartupExtensions
{
    static string GetDbConnectionString(IConfiguration config)
    {
        var host = config[Env.Postgres.Host];
        var port = config[Env.Postgres.Port];
        var db = config[Env.Postgres.Db];
        var user = config[Env.Postgres.User];
        var password = config[Env.Postgres.Password];
        var cs = $"Host={host};Port={port};Database={db};Username={user};Password={password}";
        return cs;
    }
    
    public static void InitDbContext<T>(
        this IApplicationBuilder app,
        Action<T> initAction)
        where T: DbContext
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dbContext = serviceScope
            .ServiceProvider
            .GetService<T>();

        if (dbContext != null)
        {
            initAction.Invoke(dbContext);
        }
    }

    public static IServiceCollection AddAuthenticationAndJwt(this IServiceCollection sc)
    {
        sc.AddAuthentication(configureOptions =>
            {
                configureOptions.DefaultAuthenticateScheme = AuthenticationScheme;
                configureOptions.DefaultChallengeScheme = AuthenticationScheme;
            })
            .AddJwtBearer(options => { options.ClaimsIssuer = AuthenticationScheme; });
        return sc;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();
        
        // Configure Identity to use the same JWT claims as OpenIddict instead
        // of the legacy WS-Federation claims it uses by default (ClaimTypes),
        // which saves you from doing the mapping in your authorization controller.
        services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserNameClaimType = Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = Claims.Role;
            options.ClaimsIdentity.EmailClaimType = Claims.Email;
        });
        
        return services;
    }
    
    public static IServiceCollection AddDbContext(this IServiceCollection services, 
        IConfiguration config)
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(GetDbConnectionString(config), builder => { builder.UseNodaTime(); });
            options.UseOpenIddict();
        });
        NpgsqlConnection.GlobalTypeMapper.UseNodaTime();

        return services;
    }

    public static OpenIddictBuilder AddOpenIddictServer(this IServiceCollection services, 
        IWebHostEnvironment environment)
    {
        return services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options
                    .UseEntityFrameworkCore()
                    .UseDbContext<IdentityDbContext>();
            })
            .AddServer(options =>
            {
                options
                    .AcceptAnonymousClients()
                    .AllowPasswordFlow()
                    .AllowRefreshTokenFlow();

                options
                    .SetTokenEndpointUris("/connect/token");
                
                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                var cfg = options.UseAspNetCore();
                if (environment.IsDevelopment() || environment.IsStaging())
                {
                    cfg.DisableTransportSecurityRequirement();   
                }
                
                cfg.EnableTokenEndpointPassthrough();
                
                options
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
            });
    }
}