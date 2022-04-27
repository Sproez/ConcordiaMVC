using ConcordiaLib.Domain;

namespace ConcordiaMVC.Controllers;

using ConcordiaLib.Abstract;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class CardController : Controller
{
    private readonly ILogger<CardController> _logger;
    private readonly IDbMiddleware _dbMiddleware;

    public CardController(ILogger<CardController> logger, IDbMiddleware dbMiddleware)
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
    public IActionResult CreateComment(string id)
    {
        var viewModel = new CommentCreationModel(id, "");
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment(CommentCreationModel c)
    {
        if (!ModelState.IsValid) return View(new CommentCreationModel(c.Id, ""));

        var newComment = new Comment(c.Text, DateTime.UtcNow) { Id = c.Id, CardId = c.CardId, PersonId = c.PersonId };
        try
        {
            await _dbMiddleware.PostComment(newComment);
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ChangeStatus(string id)
    {
        var lists = await _dbMiddleware.GetAllCardLists();
        var viewModel = new ListSelectionModel(id, lists);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeStatus(ListSelectionModel l)
    {
        try
        {
            await _dbMiddleware.ChangeCardStatus(l.CardId, l.SelectedId);
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

