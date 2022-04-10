using System.Linq;
using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions.Handlers;

public class UpdateQuestionHandler : IAsyncHandler<QuestionCommands.UpdateQuestionCommand, Question>
{
    private readonly IDbContextFactory<RuFlowDbContext> _contextFactory;

    public UpdateQuestionHandler(IDbContextFactory<RuFlowDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<Question> Handle(Question input)
    {
        var context = await _contextFactory.CreateDbContextAsync();
        context.Questions.Update(input);
        await context.SaveChangesAsync();
        var question = await context.Questions.Where(s => s.Id == input.Id).FirstOrDefaultAsync();
        return question ?? throw new InvalidOperationException();
    }

    public Task<Question> Handle(QuestionCommands.UpdateQuestionCommand input)
    {
        throw new NotImplementedException();
    }
}