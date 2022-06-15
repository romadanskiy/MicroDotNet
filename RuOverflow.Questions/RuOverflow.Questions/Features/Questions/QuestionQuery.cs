using Microsoft.EntityFrameworkCore;
using HotChocolate.AspNetCore.Authorization;
using RuOverflow.Questions.Base;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Questions.Requests;
using RuOverflow.Questions.Infrastructure.Handlers;

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
    public Task<List<QuestionSearchDto>> SearchQuestionsAsync(QuestionSearchRequest searchRequest,
        [Service] IAsyncHandler<QuestionSearchRequest, List<QuestionSearchDto>> handler)
    {
        return handler.Handle(searchRequest);
    }
}

