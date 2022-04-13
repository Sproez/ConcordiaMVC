namespace ConcordiaMVC.Models;

using ConcordiaLib.Domain;

public class ScientistAssignmentsModel
{
    public Person Scientist { get; set; }
    public IEnumerable<Card> Cards { get; set; }

    public ScientistAssignmentsModel(Person scientist, IEnumerable<Card> cards)
    {
        Scientist = scientist;
        Cards = cards;
    }
}

