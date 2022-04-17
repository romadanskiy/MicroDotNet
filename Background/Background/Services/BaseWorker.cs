using NCrontab;

namespace Background.Services;

public abstract class BaseWorker : BackgroundService
{
    protected readonly ILogger<BaseWorker> Logger;

    protected BaseWorker(ILogger<BaseWorker> logger)
    {
        Logger = logger;
    }

    protected abstract string Cron { get; }

    protected abstract Task RunAsync(CancellationToken stoppingToken);

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunAsync(stoppingToken);
                Logger.LogInformation($"Next run in {GetTimeBeforeNextRun().Minutes} min");
                await Task.Delay(GetTimeBeforeNextRun(), stoppingToken);
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }

    private TimeSpan GetTimeBeforeNextRun() =>
        CrontabSchedule.Parse(Cron).GetNextOccurrence(DateTime.UtcNow) - DateTime.UtcNow;
}
