namespace DAL.Entity;

public class UserGarbageFromApi
{
    public long Id { get; set; }
    public string Barcode { get; set; }
    public User User { get; set; }
}