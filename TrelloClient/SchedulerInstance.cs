using ConcordiaLib.Abstract;
using ConcordiaOrchestrator;
using Microsoft.Extensions.Logging;

namespace Scheduler;

public class SchedulerInstance : IScheduler
{
    private readonly Orchestrator _orchestrator;
    private readonly ILogger<SchedulerInstance> _logger;

    public DateTime StartingDate { get; private set; }
    public TimeSpan Interval { get; private set; }
    public TimeSpan TimeWindow { get; private set; }

    public DateTime NextExecution { get; private set; }

    public SchedulerInstance(Orchestrator o, ILogger<SchedulerInstance> logger)
    {
        _orchestrator = o;
        _logger = logger;
    }

    public async Task TestRun()
    {
        await _orchestrator.Sync();
        _logger.LogInformation("Sync done!");
    }

    public async Task ScheduleAndRun(DateTime start, TimeSpan i, TimeSpan window)
    {
        StartingDate = start;
        Interval = i;
        TimeWindow = window;

        NextExecution = StartingDate;
        while (true)
        {
            await Run();
        }
    }

    private async Task Run()
    {
        //If we missed the time window find the next one
        while (NextExecution + TimeWindow < DateTime.Now)
        {
            _logger.LogWarning($"Missed sync at {NextExecution}, retrying at {NextExecution + Interval}");
            NextExecution += Interval;
        }
        //Wait
        var remainingTime = NextExecution - DateTime.Now;
        if (remainingTime.TotalSeconds > 0) await Task.Delay(remainingTime);
        //Run
        await TrySync();
        //Plan next run
        NextExecution += Interval;
    }

    private async Task TrySync()
    {
        _logger.LogInformation($"Starting sync at {DateTime.Now}");
        try
        {
            await _orchestrator.Sync();
            _logger.LogInformation($"Sync succeded at {DateTime.Now}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Sync failed at {DateTime.Now} with exception: ");
        }
    }
}

