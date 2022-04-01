namespace RuOverflow.Questions.Infrastructure.Entity;

public abstract class CreatableEntity : Entity
{
    public DateTime Created { get; protected set; }
}
