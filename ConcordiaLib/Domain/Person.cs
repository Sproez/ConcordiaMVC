namespace ConcordiaLib.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("People")]
public record Person(
    [Required]
    string Name
    )
{
    [Key, MaxLength(50)]
    public string Id { get; init; } = null!;

    [InverseProperty("Person")]
    public List<Assignment> MyAssignments { get; init; } = null!;

    [InverseProperty("Commenter")]
    public List<Comment> MyComments { get; init; } = null!;

    public virtual bool Equals(Person? other) => Id == other?.Id && Name == other?.Name;

    public override int GetHashCode() => HashCode.Combine(Id, Name);
}