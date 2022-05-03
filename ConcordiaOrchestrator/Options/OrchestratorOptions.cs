using System.ComponentModel.DataAnnotations;

namespace ConcordiaOrchestrator.Options;

public class OrchestratorOptions
{
    [Required]
    public string DefaultDatabase { get; set; } = null!;
    //Cards on this list will be considered completed
    [Required]
    public string CompletedListId { get; set; } = null!;
}

