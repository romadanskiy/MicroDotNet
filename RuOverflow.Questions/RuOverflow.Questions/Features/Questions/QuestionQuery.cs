using Microsoft.EntityFrameworkCore;
using HotChocolate.AspNetCore.Authorization;
using RuOverflow.Questions.Base;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Questions.Models;

namespace RuOverflow.Questions.Features.Questions;

[Authorize]
[ExtendObjectType(typeof(Query))]
public class QuestionQuery
{
    [UseProjection]
    public async Task<IQueryable<Question>> GetQuestion(Guid id,
        [Service] IDbContextFactory<RuFlowDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        return context.Questions.Where(x => x.Id == id);
    }
    
    public async Task<IQueryable<Question>> GetQuestions(
        [Service] IDbContextFactory<RuFlowDbContext> contextFactory,
        [Service] IHttpContextAccessor accessor)
    {
        var context = await contextFactory.CreateDbContextAsync();
        return context.Questions.Select(s => s);
    }
}