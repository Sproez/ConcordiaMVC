using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcordiaLib.Abstract;
using ConcordiaOrchestrator;

namespace Scheduler;

public class SchedulerInstance : IScheduler
{
    private readonly Orchestrator _orchestrator;

    public DateTime StartingDate { get; private set; }
    public TimeSpan Interval { get; private set; }
    public DateTime NextExecution { get; private set; }

    public SchedulerInstance(Orchestrator o)
    {
        _orchestrator = o;
    }

    public async Task TestRun()
    {
        await _orchestrator.Sync();
    }

    public async Task ScheduleAndRun(DateTime start, TimeSpan i)
    {
        StartingDate = start;
        Interval = i;

        NextExecution = StartingDate;
        while (true)
        {
            await Run();
        }
    }

    private async Task Run()
    {
        //If we missed the time window find the next one
        while (NextExecution < DateTime.Now)
        {
            NextExecution += Interval;
        }
        //Wait
        var remainingTime = NextExecution - DateTime.Now;
        await Task.Delay(remainingTime);
        //Try sync
        Console.WriteLine($"Starting sync at {DateTime.Now}");
        try
        {
            //await _orchestrator.Sync();
            Console.WriteLine($"Sync succeded at {DateTime.Now}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Sync failed at {DateTime.Now}");
            Console.WriteLine($"Exception:");
            Console.WriteLine(e);
        }
        //Plan next run
        NextExecution += Interval;
    }

}

