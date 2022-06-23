using Cronos;
using Timer = System.Timers.Timer;

namespace Background.Jobs;

public abstract class CronJob : IHostedService, IDisposable
{
    private readonly string _expression = Environment.GetEnvironmentVariable("CRON_EXPRESSION")!;
    
    private Timer _timer;
    private readonly CronExpression _cronExpression;
    private readonly TimeZoneInfo _timeZoneInfo;

    protected CronJob()
    {
        _cronExpression = CronExpression.Parse(_expression);
        _timeZoneInfo = TimeZoneInfo.Local;
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        await ScheduleJob(cancellationToken);
    }

    protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
    {
        var next = _cronExpression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
        if (next.HasValue)
        {
            var delay = next.Value - DateTimeOffset.Now;
            if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
            {
                await ScheduleJob(cancellationToken);
            }
            _timer = new Timer(delay.TotalMilliseconds);
            _timer.Elapsed += async (_, _) =>
            {
                _timer.Dispose();  // reset and dispose timer
                _timer = null;

                if (!cancellationToken.IsCancellationRequested)
                {
                    await DoWork(cancellationToken);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    await ScheduleJob(cancellationToken);    // reschedule next
                }
            };
            _timer.Start();
        }
        await Task.CompletedTask;
    }

    public virtual async Task DoWork(CancellationToken cancellationToken) {}

    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Stop();
        await Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        _timer.Dispose();
    }
}