namespace ConcordiaTrelloClient.Dto;

using Fake;

public class CommentDto
{
    public string Id { get; init; } = null!;

    public string Text { get; init; } = null!;

    public DateTime CreatedAt { get; init; }

    public string CardId { get; init; } = null!;

    public string PersonId { get; init; } = null!;

    public CommentDto(FakeComment c) {
        Id = c.Id;
        CreatedAt = c.CreatedAt;
        PersonId = c.PersonId;

        Text = c.Data.Text;
        CardId = c.Data.Card.Id;
    }
}

