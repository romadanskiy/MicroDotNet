using RuOverflow.Questions.Features.Answers.Models;
using RuOverflow.Questions.Infrastructure.Cache;

namespace RuOverflow.Questions.Features.Answers.ObjectTypes;

public class AnswerResolvers
{
    public async Task<int> GetRating([Parent] Answer answer, [Service] ICache cache)
    {
        var cachedRating = await cache.GetAsync<int?>(answer.Id);
        await CacheRatingIfNotPresented(answer, cache, cachedRating);
        return cachedRating ?? answer.Rating;
    }

    private static async Task CacheRatingIfNotPresented([Parent] Answer answer, ICache cache, int? cachedRating)
    {
        if (cachedRating is null)
        {
            await cache.AddAsync(answer.Id, answer.Rating);
        }
    }
}
