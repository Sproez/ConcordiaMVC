using System.ComponentModel.DataAnnotations;

namespace ConcordiaOrchestrator.Options;

public class OrchestratorOptions
{
    [Required]
    public string DefaultDatabase { get; set; } = null!;
}

