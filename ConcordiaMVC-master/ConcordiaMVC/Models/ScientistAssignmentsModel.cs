namespace ConcordiaMVC.Models;

using ConcordiaLib.Domain;

public class ScientistAssignmentsModel
{
    public Person Scientist { get; set; }
    public CardPriorityModel CardModel { get; set; }

    public ScientistAssignmentsModel(Person scientist, CardPriorityModel cModel)
    {
        Scientist = scientist;
        CardModel = cModel;
    }
}

