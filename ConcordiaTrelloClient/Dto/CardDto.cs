using ConcordiaLib.Enum;

namespace ConcordiaTrelloClient.Dto;

using System;
using System.Text.Json.Serialization;

public class CardDto
{
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

}