using Microsoft.EntityFrameworkCore;

namespace DAL.Entity;

[Index(nameof(Barcode), IsUnique = true)]
public class Garbage
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? Picture { get; set; }
    
    public string Barcode { get; set; }
    
    public List<GarbageType> GarbageTypes { get; set; }
}