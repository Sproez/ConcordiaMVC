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

    public SchedulerInstance(Orchestrator o)
    {
        _orchestrator = o;
    }

    public async Task TestRun()
    {
        await _orchestrator.Sync();
    }

}

