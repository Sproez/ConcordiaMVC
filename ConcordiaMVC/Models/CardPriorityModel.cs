using ConcordiaLib.Domain;

namespace ConcordiaMVC.Models;

    public class CardPriorityModel
    {
    public IEnumerable<Card> Cards { get; init; }

    public CardPriorityModel(IEnumerable<Card> cards)
    {
        Cards = cards;
    }
}

