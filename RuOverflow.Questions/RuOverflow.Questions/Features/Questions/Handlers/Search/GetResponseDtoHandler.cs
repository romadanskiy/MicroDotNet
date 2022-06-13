using Microsoft.EntityFrameworkCore;
using RuOverflow.Questions.EF;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions.Handlers.Search
{
    public class GetResponseDtoHandler : IAsyncHandler<List<ElasticQuestionSearchDto>, List<QuestionSearchDto>>
    {
        private readonly IDbContextFactory<RuFlowDbContext> _contextFactory;

        public GetResponseDtoHandler(IDbContextFactory<RuFlowDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<QuestionSearchDto>> Handle(List<ElasticQuestionSearchDto> input)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var allTags = input.SelectMany(x => x.Tags).Distinct();
            var tags = await context.Tags
                .Where(t => allTags.Contains(t.Id))
                .ToDictionaryAsync(x => x.Id);
            return input
                .Select(e => new QuestionSearchDto()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Body = e.Body,
                    Rating = e.Rating,
                    UserId = e.UserId,
                    Created = e.Created,
                    Modified = e.Modified,
                    Tags = e.Tags.Select(x => tags[x]).ToList(),
                })
                .ToList();
        }
    }
}
