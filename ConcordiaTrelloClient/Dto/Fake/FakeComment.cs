using System.Text.Json.Serialization;

namespace ConcordiaTrelloClient.Dto.Fake;

    public class FakeComment
    {
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("data")]
    public FakeData Data { get; init; } = null!;

    [JsonPropertyName("date")]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("idMemberCreator")]
    public string PersonId { get; init; } = null!;
}