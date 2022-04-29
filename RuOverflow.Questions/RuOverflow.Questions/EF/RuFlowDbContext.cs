using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.Features.Answers.Models;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Tags.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RuOverflow.Questions.EF;

#nullable disable
public class RuFlowDbContext : IdentityDbContext<User, Role, Guid>
{
    public RuFlowDbContext(DbContextOptions<RuFlowDbContext> options) : base(options)
    {
    }

    public DbSet<Answer> Answers { get; set; }

    public DbSet<Question> Questions { get; set; }

    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasMany(x => x.Tags)
                .WithMany(x => x.Questions);

            entity.HasMany(x => x.Answers)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId);
            
            entity.HasData(Seed.Questions);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Question)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionId);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasMany(x => x.Questions)
                .WithMany(x => x.Tags);
            entity.HasAlternateKey(x => x.Name);

            entity.HasData(Seed.Tags);
        });
        

        var user = new User("Admin", "John1", "Doe2")
        {
            Id = new Guid("5f34130c-2ed9-4c83-a600-e474e8f48bac"),
            Email = "jopa@mail.ru",
            ConcurrencyStamp = "37285e0f-b3c2-4a75-85f6-73a3c4c6da29",
            PasswordHash = "AQAAAAEAACcQAAAAEED86xKz3bHadNf8B1Hg8t5qNefw4Bq1Kr2q6Jx9Ss/DcRIcUpLiFkDgQZTqUgJThA==",
            SecurityStamp = "DKBWMTFC7TZQZ6UFNZ5BN5XQNDYUBJYQ,09bd35b0-9c9f-4772-8789-e6d4b9fbe9c4"
        };
        modelBuilder.Entity<User>().HasData(user);
    }
}
