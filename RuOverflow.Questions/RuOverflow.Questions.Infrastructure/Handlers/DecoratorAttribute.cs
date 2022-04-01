namespace RuOverflow.Questions.Infrastructure.Handlers;

public class DecoratorAttribute : Attribute
{
    public DecoratorAttribute(int order)
    {
        Order = order;
    }

    public DecoratorAttribute()
    {
    }

    public int Order { get; private set; } = int.MaxValue;
}
