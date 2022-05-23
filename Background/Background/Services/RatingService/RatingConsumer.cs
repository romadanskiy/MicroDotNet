using System.Text;
using Background.Settings;
using Npgsql;

namespace Background.Services.RatingService;

public class RatingConsumer : BaseConsumerJson<ChangeRatingMessage>
{
    private readonly RatingConsumerSettings _settings;

    public RatingConsumer(KafkaSettings kafkaSettings, ILogger<RatingConsumer> logger, RatingConsumerSettings settings)
        : base(kafkaSettings, logger)
    {
        _settings = settings;
    }

    protected override string Topic => KafkaTopics.Rating;
    protected override string GroupId => ConsumerGroups.Rating;
    protected override int MessagesPerCycle => _settings.MessagesPerCycle;
    protected override string Cron => _settings.Cron;

    protected override TimeSpan MaxTimeWithoutProcessing => TimeSpan.FromMinutes(_settings.MaxTimeWithoutProcessing);

    protected override async Task ConsumeAsync(List<ChangeRatingMessage> message, CancellationToken stoppingToken)
    {
        var questions = message.Where(x => x.EntityType == EntityWithRatingType.Question)
            .GroupBy(x => x.EntityId)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Amount).Sum());

        var answers = message.Where(x => x.EntityType == EntityWithRatingType.Answer)
            .GroupBy(x => x.EntityId)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Amount).Sum());

        using var connection = new NpgsqlConnection(EnvironmentVariables.ConnectionString);
        await connection.OpenAsync(stoppingToken);
        await UpdateRatingAsync(questions, "Questions", connection, stoppingToken);
        await UpdateRatingAsync(answers, "Answers", connection, stoppingToken);
        await connection.CloseAsync();
    }

    private async Task UpdateRatingAsync(Dictionary<Guid, int> store, string tableName,
        NpgsqlConnection connection,
        CancellationToken stoppingToken)
    {
        if (!store.Any())
        {
            return;
        }

        var commandString = GetCommandString(store, tableName);
        await ExecuteCommand(connection, stoppingToken, commandString);
    }

    private async Task ExecuteCommand(NpgsqlConnection connection, CancellationToken stoppingToken,
        string commandString)
    {
        try
        {
            var command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = commandString;
            await command.ExecuteNonQueryAsync(stoppingToken);
        }
        catch (Exception exception)
        {
            Logger.LogError(exception.Message);
        }
    }

    private static string GetCommandString(Dictionary<Guid, int> store, string tableName)
    {
        var commandString =
            new StringBuilder($@"UPDATE ""{tableName}"" as t SET ""Rating"" = ""Rating"" + c.amount FROM (VALUES ");
        foreach (var entityId in store.Select(x => x.Key))
        {
            if (store.Remove(entityId, out var value))
            {
                commandString.Append($@"('{entityId}', {value}),");
            }
        }

        commandString.Remove(commandString.Length - 1, 1);
        commandString.Append(@") as c(id, amount) WHERE t.""Id"" = (c.id::uuid)");
        return commandString.ToString();
    }
}
