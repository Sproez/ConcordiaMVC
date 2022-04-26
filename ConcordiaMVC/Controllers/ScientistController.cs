namespace ConcordiaMVC.Controllers;

using ConcordiaLib.Abstract;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class ScientistController : Controller
{
    private readonly ILogger<ScientistController> _logger;
    private readonly IDbMiddleware _dbMiddleware;

    public ScientistController(ILogger<ScientistController> logger, IDbMiddleware dbMiddleware)
    {
        _logger = logger;
        _dbMiddleware = dbMiddleware;
    }

    public async Task<IActionResult> Index()
    {
        var people = await _dbMiddleware.GetAllPeople();
        var model = new ScientistSelectionModel(people);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ScientistList()
    {
        var people = await _dbMiddleware.GetAllPeople();
        var model = new ScientistSelectionModel(people);

        return View(model);
    }

    [HttpPost]
    public IActionResult ScientistList(ScientistSelectionModel model)
    {
        return RedirectToAction("ScientistAssignments", "Scientist", new { scientistId = model.SelectedId });
    }

    public async Task<IActionResult> ScientistAssignments(string scientistId)
    {
        var scientist = await _dbMiddleware.GetPerson(scientistId);
        if (scientist is null) return View("Error");
        var cards = await _dbMiddleware.GetScientistAssignments(scientistId);
        if (cards is null) return View("Error");

        var viewmodel = new ScientistAssignmentsModel(scientist, cards);
        return View(viewmodel);
    }
}

