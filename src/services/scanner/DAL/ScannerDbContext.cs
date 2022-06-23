using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ScannerDbContext : DbContext
{
    public ScannerDbContext(DbContextOptions<ScannerDbContext> options) : base(options)
    {
        
    }
    public DbSet<Garbage> Garbage { get; set; }
    
    public DbSet<GarbageReceptionPoint> GarbageReceptionPoint { get; set; }
    
    public DbSet<GarbageType> GarbageType { get; set; }
    
    public DbSet<User> User { get; set; }
    
    public DbSet<UserGarbage> UserGarbage { get; set; }

    public DbSet<UserGarbageFromApi> UserGarbageFromApi { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost; Port=5433;Database=scanner;Username=postgres;Password=postgres");
}