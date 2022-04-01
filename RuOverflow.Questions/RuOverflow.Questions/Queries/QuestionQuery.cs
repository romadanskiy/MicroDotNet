using RuOverflow.Questions.EF;
using RuOverflow.Questions.Models;

namespace RuOverflow.Questions.Queries;

public class QuestionQuery
{
    [UseDbContext(typeof(RuFlowDbContext))]
    [UseProjection]
    public IQueryable<Question> GetQuestion(Guid id, [ScopedService] RuFlowDbContext context) =>
        context.Questions.Where(x=>x.Id == id);
}
