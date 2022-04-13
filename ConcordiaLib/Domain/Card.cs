namespace ConcordiaLib.Domain;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enum;

[Table("Cards")]
public record Card
    (
    [Required]
    string Title,
    [Required]
    string Description,
    DateTime? DueBy,
    [Required]
    Priority Priority
    )
{
    [Key, MaxLength(50)]
    public string Id { get; init; } = null!;

    [InverseProperty("CardWithComment")]
    public Comment? LastComment { get; init; }

    [InverseProperty("Card")]
    public List<Assignment> Assignees { get; init; } = null!;

    public string CardListId { get; init; } = null!;
    [ForeignKey("CardListId")]
    public CardList CardList { get; init; } = null!;

    public virtual bool Equals(Card? other) =>
        Id == other?.Id &&
        Title == other?.Title &&
        Description == other?.Description &&
        DueBy == other?.DueBy &&
        Priority == other?.Priority &&
        CardListId == other?.CardListId;

    public override int GetHashCode() => HashCode.Combine(Id, Title, Description, DueBy, Priority, CardListId);
}