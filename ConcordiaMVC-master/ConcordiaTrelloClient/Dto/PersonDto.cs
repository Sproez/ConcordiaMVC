using System.Text.Json.Serialization;

namespace ConcordiaTrelloClient.Dto;

public class PersonDto
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("fullName")]
    public string Name { get; set; } = null!;
}

