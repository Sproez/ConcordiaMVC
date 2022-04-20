using System.Text.Json.Serialization;

namespace ConcordiaTrelloClient.Dto.Fake;

public class FakeData
{
    [JsonPropertyName("card")]
    public FakeCard Card { get; init; } = null!;

    [JsonPropertyName("text")]
    public string Text { get; init; } = null!;
}

