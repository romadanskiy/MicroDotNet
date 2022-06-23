using RuOverflow.Questions.Infrastructure.Cache;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Rating.Handlers;

public class ChangeRatingInCacheHandler : IAsyncHandler<ChangeRatingCommand>
{
    private readonly ICache _cache;

    public ChangeRatingInCacheHandler(ICache cache)
    {
        _cache = cache;
    }

    public async Task Handle(ChangeRatingCommand input)
    {
        var rating = await _cache.GetAsync<int?>(input.EntityId);
        if (rating is not null)
        {
            var updatedRating = rating + input.Amount;
            await _cache.AddAsync(input.EntityId, updatedRating);
        }
    }
}
