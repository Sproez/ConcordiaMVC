using ConcordiaLib.Enum;

namespace ConcordiaTrelloClient.Dto;

using System;
using System.Text.Json.Serialization;

public class CardDto
{
    //private static readonly string HighPriorityLabelId = "625032fb182ca5704dde89f7";
    //private static readonly string MediumPriorityLabelId = "62503330156b3d273dbf4bf7";
    //private static readonly string LowPriorityLabelId = "625033198048561b7029c770";

    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("name")]
    public string Title { get; init; } = null!;

    [JsonPropertyName("desc")]
    public string Description { get; init; } = null!;

    [JsonPropertyName("due")]
    public DateTime? DueBy { get; init; }

    [JsonPropertyName("idList")]
    public string CardListId { get; init; } = null!;

    [JsonPropertyName("idLabels")]
    public List<string> LabelIds { get; set; } = null!;
    /*
    public Priority Priority
    {
        get {
            if (LabelIds.Contains(HighPriorityLabelId)) return Priority.High;
            if (LabelIds.Contains(MediumPriorityLabelId)) return Priority.Medium;
            if (LabelIds.Contains(LowPriorityLabelId)) return Priority.Low;
            return Priority.Default;
        }
    }
    */


}