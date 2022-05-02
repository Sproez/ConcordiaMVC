namespace ConcordiaMVC.Models;

using ConcordiaLib.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ListSelectionModel
{
    public string CardId { get; init; }
    public string SelectedId { get; set; } = null!;
    public IEnumerable<SelectListItem> Lists { get; init; }

    public ListSelectionModel()
    {
        //MVC demands an empty constructor
        //Initialize the card ID to an invalid (non-hex) value so if it slips through the DB will raise an exception
        CardId = "ERROR_ID";
        Lists = new List<SelectListItem>();
    }

    public ListSelectionModel(string id, IEnumerable<CardList> lists)
    {
        CardId = id;
        var ListOfLists = new List<SelectListItem>();
        foreach (var l in lists)
        {
            ListOfLists.Add(new SelectListItem { Value = l.Id, Text = l.Name });
        }
        Lists = ListOfLists;
    }
}

