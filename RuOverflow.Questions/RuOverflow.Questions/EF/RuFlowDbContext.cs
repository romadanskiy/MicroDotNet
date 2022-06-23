using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.Features.Answers.Models;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Tags.Models;

namespace RuOverflow.Questions.EF;

#nullable disable
public class RuFlowDbContext : DbContext
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
    }
}
