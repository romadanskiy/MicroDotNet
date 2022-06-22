using Cronos;
using Timer = System.Timers.Timer;

namespace Background.Jobs;

public abstract class CronJob : IDisposable
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

    protected async void Start()
    {
        await ScheduleJob();
    }

    private async Task ScheduleJob()
    {
        var next = _cronExpression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
        if (!next.HasValue)
            return;
            
        var delay = next.Value - DateTimeOffset.Now;
        if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
        {
            await ScheduleJob();
            return;
        }
        _timer = new Timer(delay.TotalMilliseconds);
        _timer.Elapsed += async (_, _) =>
        {
            _timer?.Dispose();  // reset and dispose timer
            _timer = null;

            
            DoWork();
            await ScheduleJob();
        };
        _timer.Start();
    }

    protected virtual void DoWork() {}

    protected async Task StopAsync()
    {
        _timer.Stop();
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}