using RuOverflow.Questions.Infrastructure.Requests;

namespace RuOverflow.Questions.Features.Questions.Requests
{
    public class QuestionSearchRequest
    {
        public QuestionFilters? Filters { get; set; }

        public Paging Paging { get; set; } = new();
    }
}
