namespace RuOverflow.Questions.Infrastructure.Entity;

public abstract class ModifiableEntity : CreatableEntity
{
    public DateTime? Modified { get; set; }
}
