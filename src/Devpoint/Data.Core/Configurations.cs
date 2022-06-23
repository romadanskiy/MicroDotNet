using Domain.Developers.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Core;

public static class Configurations
{
    public static void Execute(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>()
            .HasOne(company => company.Owner)
            .WithMany(owner => owner.OwnedCompanies);
        modelBuilder.Entity<Company>()
            .HasMany(company => company.Developers)
            .WithMany(developer => developer.Companies);
        
        modelBuilder.Entity<Project>()
            .HasOne(project => project.Owner)
            .WithMany(owner => owner.OwnedProjects);
        modelBuilder.Entity<Project>()
            .HasMany(project => project.Developers)
            .WithMany(developer => developer.Projects);
    }
}