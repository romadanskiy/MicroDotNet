using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ScannerDbContext : DbContext
{
    public DbSet<Garbage> Garbage { get; set; }
    
    public DbSet<GarbageReceptionPoint> GarbageReceptionPoint { get; set; }
    
    public DbSet<GarbageType> GarbageType { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost; Port=5433;Database=scanner;Username=postgres;Password=postgres");
}