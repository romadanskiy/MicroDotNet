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
        private readonly ILogger<QuestionConsumer> _logger;

        public QuestionConsumer(KafkaSettings kafkaSettings, ILogger<QuestionConsumer> logger,
            QuestionConsumerSettings settings, ElasticClient elasticClient) :
            base(kafkaSettings, logger)
        {
            _logger = logger;
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
            var response = await _elasticClient.Indices.ExistsAsync(new IndexExistsRequest("questions"));
            if (!response.Exists || true)
            {
                await _elasticClient.Indices
                    .CreateAsync("questions",
                        descriptor => descriptor
                            .Settings(s => s
                                .NumberOfShards(1)
                                .NumberOfReplicas(0)
                            )
                            .Map<Question>(x => x.AutoMap()));
            }
        }

        protected override async Task ConsumeAsync(List<DebeziumPayload> messages, CancellationToken stoppingToken)
        {
            var questions = GetQuestions(messages);
            //todo переделать через подтягивание топика(может с KSql)
            await FillTags(messages, stoppingToken, questions);
            var questionList = questions.Select(x => x.Value!);

            var observable = _elasticClient.BulkAll(questionList,
                b => b
                    .Index("questions")
                    .BackOffRetries(2)
                    .BackOffTime("30s")
                    .RefreshOnCompleted()
                    .MaxDegreeOfParallelism(4)
                    .Size(1000)
            );

            observable.Subscribe(new BulkAllObserver(
                onNext: response => _logger.LogInformation("Completed indexing"),
                onError: e => _logger.LogError(e.Message)
            ));
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
