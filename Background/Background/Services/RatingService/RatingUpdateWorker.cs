using System.Collections.Concurrent;
using System.Text;
using Background.Settings;
using Npgsql;

namespace Background.Services.RatingService;

public class RatingUpdateWorker : BaseWorker
{
    public RatingUpdateWorker(RatingUpdateWorkerSettings settings, ILogger<RatingUpdateWorker> logger) : base(logger)
    {
        Cron = settings.Cron;
    }

    protected override string Cron { get; }

    protected override async Task RunAsync(CancellationToken stoppingToken)
    {
        using var connection = new NpgsqlConnection(EnvironmentVariables.ConnectionString);
        await connection.OpenAsync(stoppingToken);
        await UpdateRatingAsync(RatingStore.Questions, "Questions", connection, stoppingToken);
        await UpdateRatingAsync(RatingStore.Answers, "Answers", connection, stoppingToken);
        await connection.CloseAsync();
    }

    private async Task UpdateRatingAsync(ConcurrentDictionary<Guid, int> store, string tableName,
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

    private static string GetCommandString(ConcurrentDictionary<Guid, int> store, string tableName)
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
