using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Answers.Models;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Answers.Handlers;

public class AddAnswerHandler : IAsyncHandler<AnswerCommands.AddAnswerCommand, Answer>
{
    private readonly IDbContextFactory<RuFlowDbContext> _dbContext;

    public AddAnswerHandler(IDbContextFactory<RuFlowDbContext> dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Answer> Handle(AnswerCommands.AddAnswerCommand input)
    {
        var context = await _dbContext.CreateDbContextAsync();

        //todo брать из jwt
        var userId = Guid.NewGuid();
        var answer = new Answer(input.QuestionId, input.Body, userId);
        context.Answers.Add(answer);
        await context.SaveChangesAsync();
        return answer;
    }
}
