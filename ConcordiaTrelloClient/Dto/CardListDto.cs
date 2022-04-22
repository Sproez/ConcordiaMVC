namespace ConcordiaTrelloClient.Dto;

using System.Text.Json.Serialization;

public class CardListDto
{
    //name must be unique, so it can be used as ID :))))))))
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
    //the trello id is just a name for our DB :))))))))))))))))))))))))))))))))
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
}

