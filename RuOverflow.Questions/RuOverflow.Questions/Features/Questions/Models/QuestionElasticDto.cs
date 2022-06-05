namespace RuOverflow.Questions.Features.Questions.Models
{
    public class QuestionElasticDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public List<Guid> Tags { get; set; } = new();
    }
}
