using Nest;
using RuOverflow.Questions.Features.Questions.Models;
using RuOverflow.Questions.Features.Questions.Requests;
using RuOverflow.Questions.Infrastructure.ElasticSearch;
using RuOverflow.Questions.Infrastructure.ElasticSearch.Extensions;
using RuOverflow.Questions.Infrastructure.Handlers;

namespace RuOverflow.Questions.Features.Questions.Handlers.Search
{
    public class FetchElasticHandler : IAsyncHandler<QuestionSearchRequest, List<ElasticQuestionSearchDto>>
    {
        private readonly ElasticClient _elasticClient;

        public FetchElasticHandler(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<List<ElasticQuestionSearchDto>> Handle(QuestionSearchRequest input)
        {
            var documents = await _elasticClient.SearchAsync<ElasticQuestionSearchDto>(
                main => main
                    .Index(IndexNames.Question)
                    .Query(
                        q =>
                            SearchText(q, input.Filters?.SearchInput) &&
                            q.MatchMany(f => f.Tags, input.Filters?.Tags)
                    )
                    .Skip((input.Paging.Page - 1) * input.Paging.Size)
                    .Take(input.Paging.Size)
            );
            return documents.Documents.ToList();
        }

        private static QueryContainer SearchText(QueryContainerDescriptor<ElasticQuestionSearchDto> q, string? input)
        {
            return q
                .MultiMatch(f => f
                    .Fields(x => x
                        .Field(e => e.Body)
                        .Field(e => e.Title, 2)
                    )
                    .Fuzziness(Fuzziness.Auto)
                    .Query(input)
                );
        }
    }
}
