namespace ConcordiaLib.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Comments")]
public record Comment(
    [Required]
    string Text,
    [Required]
    DateTime CreatedAt
    )
{
    [Key, MaxLength(50)]
    public string Id { get; init; } = null!;

    public string CardId { get; init; } = null!;
    [ForeignKey("CardId")]
    public Card CardWithComment { get; init; } = null!;

    public string PersonId { get; init; } = null!;
    [ForeignKey("PersonId")]
    public Person Commenter { get; init; } = null!;

    public virtual bool Equals(Comment? other) => 
        Id == other?.Id &&
        Text == other?.Text &&
        CreatedAt == other?.CreatedAt &&
        CardId == other?.CardId &&
        PersonId == other?.PersonId;

    public override int GetHashCode() => HashCode.Combine(Id, Text, CreatedAt, CardId, PersonId);
}