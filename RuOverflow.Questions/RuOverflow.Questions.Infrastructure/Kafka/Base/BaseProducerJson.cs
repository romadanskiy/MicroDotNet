using Confluent.Kafka;
using Newtonsoft.Json;

namespace RuOverflow.Questions.Infrastructure.Kafka.Base;

public abstract class BaseProducerJson<T> : IProducer<T>
{
    private readonly IProducer<Null, string> _producer;

    public BaseProducerJson(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    protected abstract string Topic { get; }

    public virtual async Task Publish(T message)
    {
        await _producer.ProduceAsync(Topic, new Message<Null, string>()
        {
            Value = JsonConvert.SerializeObject(message),
        });
    }
}
