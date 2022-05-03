namespace ConcordiaLib.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Assignments")]
public record Assignment()
{
    [Key]
    [Column(Order = 1)]
    public string CardId { get; init; } = null!;
    [ForeignKey("CardId")]
    public Card Card { get; init; } = null!;

    [Key]
    [Column(Order = 2)]
    public string PersonId { get; init; } = null!;
    [ForeignKey("PersonId")]
    public Person Person { get; init; } = null!;

    public virtual bool Equals(Assignment? other) => CardId == other?.CardId && PersonId == other?.PersonId;

    public override int GetHashCode() => HashCode.Combine(CardId, PersonId);
}