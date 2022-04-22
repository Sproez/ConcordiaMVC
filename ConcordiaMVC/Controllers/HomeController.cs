namespace ConcordiaMVC.Controllers;

using ConcordiaLib.Abstract;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDbMiddleware _dbMiddleware;

    public HomeController(ILogger<HomeController> logger, IDbMiddleware dbMiddleware)
    {
        _logger = logger;
        _dbMiddleware = dbMiddleware;
    }

    public async Task<IActionResult> Index()
    {
        var cards = await _dbMiddleware.GetAllCards();
        var result = new CardPriorityModel(cards.OrderByDescending(c => c.Priority));
        return View(result);
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
        return RedirectToAction("ScientistAssignments", "Home", new { scientistId = model.SelectedId });
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
