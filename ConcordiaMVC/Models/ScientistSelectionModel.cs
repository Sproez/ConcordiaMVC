using ConcordiaLib.Domain;

namespace ConcordiaMVC.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

public class ScientistSelectionModel
{
    public string SelectedId { get; set; } = null!;
    public IEnumerable<SelectListItem> Scientists { get; init; }

    public ScientistSelectionModel()
    {
         Scientists = new List<SelectListItem>();
    }

    public ScientistSelectionModel(IEnumerable<Person> people)
    {
        var SList = new List<SelectListItem>();
        foreach (var person in people)
        {
            SList.Add(new SelectListItem { Value = person.Id, Text = person.Name });
        }
        Scientists = SList;
    }
}


