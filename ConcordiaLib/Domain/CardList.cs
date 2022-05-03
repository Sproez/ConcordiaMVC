namespace ConcordiaLib.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CardLists")]
public record CardList(
    [Required]
    string Name
    )
{
    [Key]
    public string Id { get; init; } = null!;

    [InverseProperty("CardList")]
    public List<Card> Cards { get; init; } = null!;

    public virtual bool Equals(CardList? other) => Id == other?.Id && Name == other?.Name;

    public override int GetHashCode() => HashCode.Combine(Id, Name);
}