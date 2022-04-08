namespace RuOverflow.Questions.Features.Rating;

public interface IHasRating
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
