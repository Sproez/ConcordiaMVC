using System.ComponentModel.DataAnnotations;
using ConcordiaLib.Domain;
using ConcordiaLib.Enum;

namespace ConcordiaWebApi.Dtos;

public class CardDto
{
    [Required]
    public string Id { get; init; }
    [Required]
    public string Title { get; init; }
    [Required]
    public string Description { get; init; }

    public DateTime? DueBy { get; init; }
    [Required]
    public Priority Priority { get; init; }
    [Required]
    public string Status { get; init; }
    


    public CardDto(Card c)
    {
        Id = c.Id;
        Title = c.Title;
        Description = c.Description;
        DueBy = c.DueBy;
        Priority = c.Priority;
        Status = c.CardList?.Name ?? "ERROR";
    }
}

