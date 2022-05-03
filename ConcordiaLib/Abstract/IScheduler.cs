namespace ConcordiaLib.Abstract;

public interface IScheduler
{
    Task TestRun();
    Task ScheduleAndRun(DateTime start, TimeSpan i, TimeSpan window);
}