namespace ConcordiaTrelloClient.Dto;

using System.Text.Json.Serialization;
using NestedProperties;

public class CommentDto
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("data")]
    public NestedData Data { get; init; } = null!;

    public string Text => this.Data.Text;

    [JsonPropertyName("date")]
    public DateTime CreatedAt { get; init; }

    public string CardId => this.Data.Card.Id;

    [JsonPropertyName("idMemberCreator")]
    public string PersonId { get; init; } = null!;

    /*
    public CommentDto(FakeComment c) {
        Id = c.Id;
        CreatedAt = c.CreatedAt;
        PersonId = c.PersonId;

        Text = c.Data.Text;
        CardId = c.Data.Card.Id;
    }
    */
}

