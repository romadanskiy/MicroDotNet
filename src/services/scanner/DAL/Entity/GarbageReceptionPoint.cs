namespace DAL.Entity;

public class GarbageReceptionPoint
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string Address { get; set; }
    
    public List<GarbageType> GarbageTypes { get; set; }
}