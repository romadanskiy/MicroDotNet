using System.Net;
using Confluent.Kafka;

namespace RuOverflow.Questions.Features.Rating.Producer;

record RatingMessage(Guid EntityId, EntityWithRatingType EntityType, int Amount)
    : ChangeRatingCommand(EntityId, EntityType, Amount);

public class RatingProducer
{
    public Task Publish(ChangeRatingCommand message)
    {
        
    }
}
