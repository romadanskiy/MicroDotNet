namespace RuOverflow.Questions.Infrastructure.Entity;

public class ModifiableEntity : CreatableEntity
{
    public DateTime? Modified { get; protected set; }
}
