using RuOverflow.Questions.Infrastructure.Cache;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Rating.Handlers;

[Decorator]
public class CacheDecorator : IHandler<RatingCommands.ChangeRatingCommand, IHasRating>
{
    private readonly ICache _cache;
    private readonly IHandler<RatingCommands.ChangeRatingCommand, IHasRating> _handler;

    public CacheDecorator(IHandler<RatingCommands.ChangeRatingCommand, IHasRating> handler, ICache cache)
    {
        _handler = handler;
        _cache = cache;
    }

    public IHasRating Handle(RatingCommands.ChangeRatingCommand input)
    {
        _cache.AddAsync(input.Entity)
    }
}
