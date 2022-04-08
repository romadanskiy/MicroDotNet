using RuOverflow.Questions.Infrastructure.Cache;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Rating.Handlers;

public class CacheDecorator : IAsyncHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity>
{
    private readonly ICache _cache;
    private readonly IHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity> _handler;

    public CacheDecorator(IHandler<RatingCommands.ChangeRatingCommand, HasRatingEntity> handler, ICache cache)
    {
        _handler = handler;
        _cache = cache;
    }

    public async Task<HasRatingEntity> Handle(RatingCommands.ChangeRatingCommand input)
    {
        var entity = _handler.Handle(input);
        await _cache.AddAsync(entity.Id, entity.Rating);
        return entity;
    }
}
