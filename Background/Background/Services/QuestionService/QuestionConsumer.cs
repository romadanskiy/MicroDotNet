using System.Text;
using Background.Services.QuestionService.Models;
using Background.Settings;
using Nest;
using Npgsql;

namespace Background.Services.QuestionService
{
    public class QuestionConsumer : BaseConsumerJson<DebeziumPayload>
    {
        private readonly QuestionConsumerSettings _settings;
        private readonly ElasticClient _elasticClient;

        public QuestionConsumer(KafkaSettings kafkaSettings, ILogger<QuestionConsumer> logger,
            QuestionConsumerSettings settings, ElasticClient elasticClient) :
            base(kafkaSettings, logger)
        {
            _settings = settings;
            _elasticClient = elasticClient;
        }

        protected override string Topic => KafkaTopics.Questions;
        protected override string GroupId => ConsumerGroups.Questions;
        protected override int MessagesPerCycle => _settings.MessagesPerCycle;
        protected override string Cron => _settings.Cron;

        protected override TimeSpan MaxTimeWithoutProcessing =>
            TimeSpan.FromMinutes(_settings.MaxTimeWithoutProcessing);

        protected override async Task BeforeStart()
        {
            await _elasticClient.Indices.CreateAsync("questions",
                descriptor => { return descriptor.Map<Question>(x => x.AutoMap()); });
        }

        protected override async Task ConsumeAsync(List<DebeziumPayload> messages, CancellationToken stoppingToken)
        {
            var questions = GetQuestions(messages);
            //todo переделать через подтягивание топика(может с KSql)
            await FillTags(messages, stoppingToken, questions);
            var questionList = questions.Select(x => x.Value);

            await _elasticClient.BulkAsync(b => b
                .Index("questions")
                .IndexMany(questionList), stoppingToken);
        }

        private static async Task FillTags(List<DebeziumPayload> messages, CancellationToken stoppingToken,
            Dictionary<Guid, Question?> questions)
        {
            var sql = GetSqlQuery(messages);

            using var connection = new NpgsqlConnection(EnvironmentVariables.ConnectionString);
            await connection.OpenAsync(stoppingToken);
            var command = connection.CreateCommand();

            command.CommandText = sql;
            var reader = await command.ExecuteReaderAsync(stoppingToken);
            while (await reader.ReadAsync(stoppingToken))
            {
                var questionId = reader.GetGuid(0);
                if (questions.TryGetValue(questionId, out var question))
                {
                    question!.Tags.Add(reader.GetGuid(1));
                }
            }

            await connection.CloseAsync();
        }

        private static string GetSqlQuery(List<DebeziumPayload> messages)
        {
            var sql = new StringBuilder(@"SELECT * from ""QuestionTag"" WHERE ");
            foreach (var message in messages)
            {
                sql.Append(@$"""QuestionsId"" = '{message.Payload.After.Id}' OR");
            }

            sql.Remove(sql.Length - 3, 3);
            sql.Append(';');
            return sql.ToString();
        }

        private static Dictionary<Guid, Question?> GetQuestions(List<DebeziumPayload> messages)
        {
            return messages
                .Select(x => x.Payload)
                .Select(x => x.After)
                .GroupBy(x => x.Id)
                .Select(x => x.MaxBy(y => y.Modified))
                .ToDictionary(x => x!.Id);
        }
    }
}
