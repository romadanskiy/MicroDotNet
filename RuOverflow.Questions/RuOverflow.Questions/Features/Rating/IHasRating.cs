using RuOverflow.Questions.Infrastructure.Entity;

namespace RuOverflow.Questions.Features.Rating;

public abstract class HasRatingEntity : ModifiableEntity
{
    public int Rating { get; protected set; }

    public void Like()
    {
        Rating++;
    }

    public void Dislike()
    {
        Rating--;
    }
}
