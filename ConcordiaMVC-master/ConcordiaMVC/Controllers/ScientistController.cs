using ConcordiaLib.Utils;

namespace ConcordiaMVC.Controllers;

using Microsoft.Extensions.Options;
using Options;
using ConcordiaLib.Abstract;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class ScientistController : Controller
{
    private readonly ILogger<ScientistController> _logger;
    private readonly MyMvcOptions _options;
    private readonly IDbMiddleware _dbMiddleware;
    private readonly CardComparer _cardComparer;

    public ScientistController(ILogger<ScientistController> logger, IOptions<MyMvcOptions> options, IDbMiddleware dbMiddleware, CardComparer cardComparer)
    {
        _logger = logger;
        _options = options.Value;
        _dbMiddleware = dbMiddleware;
        _cardComparer = cardComparer;
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

        var cardsOrdered = cards.OrderBy(c => c, _cardComparer);
        var cModel = new CardPriorityModel(cardsOrdered, _options);

        var viewModel = new ScientistAssignmentsModel(scientist, cModel);
        return View(viewModel);
    }
}

