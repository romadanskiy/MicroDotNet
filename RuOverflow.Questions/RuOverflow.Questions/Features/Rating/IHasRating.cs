using RuOverflow.Questions.Infrastructure.Entity;

namespace RuOverflow.Questions.Features.Rating;

public abstract class HasRatingEntity : ModifiableEntity
{
    public int Rating { get; set; }

    public void Like()
    {
        Rating++;
    }

    public void Dislike()
    {
        Rating--;
    }
}
