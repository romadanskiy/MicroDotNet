namespace Scanner.Models;

public class ReceptionPointDto
{
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public string Address { get; set; }
    
    public string Longitude { get; set; }
    
    public string Latitude { get; set; }
    
    public List<long> GarbageTypeIds { get; set; }
}