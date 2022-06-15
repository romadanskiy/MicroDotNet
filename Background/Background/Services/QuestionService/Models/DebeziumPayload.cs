namespace Background.Services.QuestionService.Models
{
    public class DebeziumPayload
    {
        public Payload Payload { get; set; }
    }

    public class Payload
    {
        public Question? Before { get; set; }
        public Question? After { get; set; }
    }
}
