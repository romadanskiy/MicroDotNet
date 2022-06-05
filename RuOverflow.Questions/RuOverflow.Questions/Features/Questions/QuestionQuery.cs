using Microsoft.EntityFrameworkCore;
using Nest;
using RuOverflow.Questions.Base;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Extensions;

namespace RuOverflow.Questions.Features.Questions;

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

    public async Task<List<Question>> GetQuestions(
        [Service] IDbContextFactory<RuFlowDbContext> contextFactory, [Service] ElasticClient elasticClient)
    {
        var documents = await elasticClient.SearchAsync<QuestionElasticDto>(
            main => main.Query(
                q =>
                    SearchText(q) &&
                    MatchUserId(q) &&
                    q.MatchMany(f => f.Tags, new[] { "a792b957-7ec5-4eed-ad03-cb91c702b098" })
            ));
    }

    private static QueryContainer MatchUserId(QueryContainerDescriptor<QuestionElasticDto> q)
    {
        return q.Match(s => s.Field(f => f.UserId).Query("1faa65e1-6daa-4935-b868-a89b7972345c"));
    }

    private static QueryContainer SearchText(QueryContainerDescriptor<QuestionElasticDto> q)
    {
        return q.MultiMatch(f => f
            .Fields(x => x
                .Field(e => e.Body).Field(e => e.Title, 2)
            )
            .Query("Как запустить docker?")
            .Type(TextQueryType.Phrase)
        );
    }
}
