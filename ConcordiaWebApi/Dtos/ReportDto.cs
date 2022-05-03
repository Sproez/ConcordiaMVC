namespace ConcordiaWebApi.Dtos;

public class ReportDto
{
    public string ScientistId { get; init; }
    public int CompletedTasks { get; init; }
    public int AssignedTasks { get; init; }
    public string PercentCompleted => AssignedTasks != 0 ? $"{((float)CompletedTasks / (float)AssignedTasks):P0}" : "N/A";

    public ReportDto(string p, int c, int a)
    {
        ScientistId = p;
        CompletedTasks = c;
        AssignedTasks = a;
    }
}

