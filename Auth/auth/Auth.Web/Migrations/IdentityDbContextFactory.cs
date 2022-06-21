using System;
using System.IO;
using AuthorizationServer.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuthorizationServer.Web.Migrations;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        Env.LoadFromCurrentDirectory();

        var host = Env.Get(Env.Postgres.Host);
        var port = Env.Get(Env.Postgres.Port);
        var db = Env.Get(Env.Postgres.Db);
        var user = Env.Get(Env.Postgres.User);
        var password = Env.Get(Env.Postgres.Password);

        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
        optionsBuilder.UseNpgsql(
            $"Host={host};Port={port};Database={db};Username={user};Password={password}",
            builder => { builder.UseNodaTime(); });

        return new IdentityDbContext(optionsBuilder.Options);
    }
}