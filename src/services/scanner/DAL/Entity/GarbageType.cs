namespace DAL.Entity;

public class GarbageType
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public List<GarbageReceptionPoint> GarbageReceptionPoints { get; set; }
    
    public List<Garbage> Garbages { get; set; }
}