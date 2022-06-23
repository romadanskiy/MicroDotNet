using Confluent.Kafka.Admin;

namespace RuOverflow.Questions.Infrastructure.Kafka.Helpers;

public class CreateTopicOptionsComparer : IEqualityComparer<CreateTopicsOptions>
{
    public bool Equals(CreateTopicsOptions? x, CreateTopicsOptions? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.ValidateOnly == y.ValidateOnly && Nullable.Equals(x.RequestTimeout, y.RequestTimeout) &&
               Nullable.Equals(x.OperationTimeout, y.OperationTimeout);
    }

    public int GetHashCode(CreateTopicsOptions obj)
    {
        return HashCode.Combine(obj.ValidateOnly, obj.RequestTimeout, obj.OperationTimeout);
    }
}
