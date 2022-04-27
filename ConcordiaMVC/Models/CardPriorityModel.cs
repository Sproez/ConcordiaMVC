namespace ConcordiaMVC.Models;

using ConcordiaLib.Domain;
using ConcordiaLib.Enum;
using Options;

public class CardPriorityModel
{
    public IEnumerable<Card> Cards { get; init; }
    private MyMvcOptions _options { get; init; }

    public CardPriorityModel(IEnumerable<Card> cards, MyMvcOptions options)
    {
        Cards = cards;
        _options = options;
    }

    public string GetPriorityClass(Card c)
    {
        if (c.CardListId == _options.CompletedListId) return "Completed-Task";
        return c.Priority switch
        {
            Priority.High => "High-Priority",
            Priority.Medium => "Medium-Priority",
            Priority.Low => "Low-Priority",
            _ => "Low-Priority",
        };
    }
}

