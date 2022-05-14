using System.Collections.Concurrent;
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
        foreach (var entityId in store.Select(x => x.Key))
        {
            if (store.Remove(entityId, out var value))
            {
                try
                {
                    //todo убрать обнвление бд в цикле
                    await ExecuteDatabaseUpdateQueryAsync(connection, tableName, entityId, value, stoppingToken);
                }
                catch (Exception exception)
                {
                    Logger.LogError(exception.Message);
                    store.TryAdd(entityId, value);
                }
            }
        }
    }

    private static async Task ExecuteDatabaseUpdateQueryAsync(NpgsqlConnection connection, string tableName,
        Guid entityId, int value, CancellationToken stoppingToken)
    {
        var command = new NpgsqlCommand();
        var amount = new NpgsqlParameter<int>("@amount", value);
        var id = new NpgsqlParameter<Guid>("@id", entityId);
        command.Connection = connection;
        command.Parameters.Add(amount);
        command.Parameters.Add(id);
        command.CommandText =
            $@"UPDATE ""{tableName}""
                        SET ""Rating"" = ""Rating"" + @amount
                        WHERE ""Id"" = @id;";
        await command.ExecuteNonQueryAsync(stoppingToken);
    }
}
