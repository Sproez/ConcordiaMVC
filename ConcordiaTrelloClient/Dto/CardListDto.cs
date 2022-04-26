namespace ConcordiaTrelloClient.Dto;

using System.Text.Json.Serialization;

public class CardListDto
{
    
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
   
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
}

