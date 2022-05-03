using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConcordiaLib.Domain;

[Table("PerformanceReport")]
[Keyless]
public record Report(
    [Required]
    string Name,
    [Required] [property: Column("CompletedAssignments")]
    int completedTasks, 
    [Required] [property: Column("Assignments")]
    int assignedTasks
    )
{
    [NotMapped]
    public string PercentCompleted => assignedTasks != 0 ? $"{((float)completedTasks / (float)assignedTasks):P0}" : "N/A";
}

