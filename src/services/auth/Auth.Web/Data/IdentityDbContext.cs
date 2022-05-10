using System;
using AuthorizationServer.Web.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationServer.Web.Data;

public class IdentityDbContext : IdentityDbContext<User, Role, Guid>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.UseOpenIddict();

        var user = new User("Admin", "John1", "Doe2")
        {
            Id = new Guid("5f34130c-2ed9-4c83-a600-e474e8f48bac"), 
            CreatedAt = Dates.Now(),
            ConcurrencyStamp = "37285e0f-b3c2-4a75-85f6-73a3c4c6da29",
            PasswordHash = "AQAAAAEAACcQAAAAEED86xKz3bHadNf8B1Hg8t5qNefw4Bq1Kr2q6Jx9Ss/DcRIcUpLiFkDgQZTqUgJThA==",
            SecurityStamp = "DKBWMTFC7TZQZ6UFNZ5BN5XQNDYUBJYQ,09bd35b0-9c9f-4772-8789-e6d4b9fbe9c4"
        };
        builder.Entity<User>().HasData(user);
    }
}