namespace ConcordiaTrelloClient.Dto.NestedProperties;

using System.Text.Json.Serialization;

public class NestedCard
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("idMembers")]
    public List<string> AssigneesIds { get; set; } = null!;
}

