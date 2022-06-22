using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using RatingGrpcService;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Extensions;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions.Handlers.Ask;

public record AskQuestionCommand(string Title, string Body, List<Guid>? Tags);

public class AskQuestionHandler : IAsyncHandler<AskQuestionCommand, Question>
{
    private readonly IDbContextFactory<RuFlowDbContext> _contextFactory;
    private readonly IHttpContextAccessor _accessor;

    public AskQuestionHandler([Service] IHttpContextAccessor accessor ,IDbContextFactory<RuFlowDbContext> contextFactory)
    {
        _accessor = accessor;
        _contextFactory = contextFactory;
    }

    public async Task<Question> Handle(AskQuestionCommand input)
    {
        var context = await _contextFactory.CreateDbContextAsync();
        var tags = input.Tags?.Count > 0
            ? await context.Tags.Where(x => input.Tags.Contains(x.Id)).ToListAsync()
            : null;
        
        var userId = _accessor.GetUserId();
        var question = new Question(input.Title, input.Body, userId, tags);
        
        question.Rating = 4;
        
        context.Questions.Add(question);
        await context.SaveChangesAsync();
        return question;
    }
}
