using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcordiaLib.Abstract;

public interface IScheduler
{
    Task TestRun();
    Task ScheduleAndRun(DateTime start, TimeSpan i, TimeSpan window);
}