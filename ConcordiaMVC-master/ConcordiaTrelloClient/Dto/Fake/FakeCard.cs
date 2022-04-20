using System.Text.Json.Serialization;

namespace ConcordiaTrelloClient.Dto.Fake;

public class FakeCard
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("idMembers")]
    public List<string> AssigneesIds { get; set; } = null!;
}

