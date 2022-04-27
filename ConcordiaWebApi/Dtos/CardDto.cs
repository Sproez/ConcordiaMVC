using System.ComponentModel.DataAnnotations;
using ConcordiaLib.Domain;
using ConcordiaLib.Enum;

namespace ConcordiaWebApi.Dtos;

public class CardDto : IComparable<CardDto>
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

    public int CompareTo(CardDto? other)
    {
        if (other is null) return -1;
        var fiveDays = new TimeSpan(5, 0, 0, 0);

        //Priority values:
        //High : 4
        //Due by in <5 days : 3
        //Medium : 2
        //Low : 1
        //Default : 0
        //Completed : -1

        int HighP(CardDto c) => c.Priority == Priority.High ? 1 : 0;
        bool DueEarly(CardDto c) => c.DueBy is not null && (c.DueBy - DateTime.Now) < fiveDays && c.Priority != Priority.High;

        int thisP = (int)this.Priority + HighP(this);
        thisP = DueEarly(this) ? 3 : thisP;
        int otherP = (int)other.Priority + HighP(other);
        otherP = DueEarly(other) ? 3 : otherP;

        return otherP - thisP;
    }
}

