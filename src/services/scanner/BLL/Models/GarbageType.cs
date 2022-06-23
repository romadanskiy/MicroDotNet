namespace BLL.DTO;

public class GarbageType
{
    public long Id { get; }
    public string Name { get; }

    public GarbageType(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public GarbageType(long id)
    {
        Id = id;
    }
}