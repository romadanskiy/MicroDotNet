namespace RuOverflow.Questions.Infrastructure.Exceptions;

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(Guid id)
        : base($"No entity with id = {id}")
    {
    }
}
