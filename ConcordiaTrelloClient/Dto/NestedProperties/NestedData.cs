namespace ConcordiaTrelloClient.Dto.NestedProperties;

using System.Text.Json.Serialization;

public class NestedData
{
    [JsonPropertyName("card")]
    public NestedCard Card { get; init; } = null!;

    [JsonPropertyName("text")]
    public string Text { get; init; } = null!;
}

