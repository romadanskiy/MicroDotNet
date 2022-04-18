using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Answers.Models;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Cache;

namespace RuOverflow.Questions.Features.Questions.ObjectTypes;

public class QuestionResolvers
{
    [UsePaging, UseProjection, UseFiltering, UseSorting]
    public async Task<IQueryable<Answer>> GetAnswers([Parent] Question question,
        [Service] IDbContextFactory<RuFlowDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        return context.Answers.Where(x => x.QuestionId == question.Id);
    }

    public async Task<int> GetRating([Parent] Question question, [Service] ICache cache)
    {
        var cachedRating = await cache.GetAsync<int?>(question.Id);
        await CacheRatingIfNotPresented(question, cache, cachedRating);
        return cachedRating ?? question.Rating;
    }

    private static async ValueTask CacheRatingIfNotPresented(Question question, ICache cache, int? cachedRating)
    {
        if (cachedRating is null) await cache.AddAsync(question.Id, question.Rating);
    }
}
