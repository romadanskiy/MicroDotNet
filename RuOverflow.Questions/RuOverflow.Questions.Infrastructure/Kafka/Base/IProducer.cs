namespace RuOverflow.Questions.Infrastructure.Kafka.Base;

public interface IProducer<in T>
{
    public abstract Task Publish(T message);
}
