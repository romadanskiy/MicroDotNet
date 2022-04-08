using RuOverflow.Questions.Base;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Questions.Models;

namespace RuOverflow.Questions.Features.Questions;

[ExtendObjectType(typeof(Query))]
public class QuestionQuery
{
    [UseDbContext(typeof(RuFlowDbContext))]
    [UseProjection]
    public IQueryable<Question> GetQuestion(Guid id, [ScopedService] RuFlowDbContext context) =>
        context.Questions.Where(x => x.Id == id);
}
