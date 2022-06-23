using System.Text.Json.Serialization;

namespace Scanner.Models;

public class AddGarbageInfoDto
{
    public IFormFile? Image { get; set; }
    public string Name { get; set; }
    public string Barcode { get; set; }
    public string? Description { get; set; }
    public List<long> GarbageTypes { get; set; }
}