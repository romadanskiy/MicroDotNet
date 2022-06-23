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
        => optionsBuilder.UseNpgsql("Host=ec2-52-30-67-143.eu-west-1.compute.amazonaws.com; Port=5432;Database=dcrpp0uvmtu58g;Username=blqpsxkjotlydp;Password=16d350dc99df42807d380b26051daa9ebd80d92da7d1ecc274c303db869968f6");
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseNpgsql("localhost; Port=5433;Database=scanner;Username=postgres;Password=postgres");
}