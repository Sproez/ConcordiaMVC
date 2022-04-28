using ConcordiaLib.Utils;

namespace ConcordiaMVC.Controllers;

using Options;
using Models;
using ConcordiaLib.Domain;
using Microsoft.Extensions.Options;
using ConcordiaLib.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class CardController : Controller
{
    private readonly ILogger<CardController> _logger;
    private readonly MyMvcOptions _options;
    private readonly IDbMiddleware _dbMiddleware;
    private readonly CardComparer _cardComparer;

    public CardController(ILogger<CardController> logger, IOptions<MyMvcOptions> options, IDbMiddleware dbMiddleware, CardComparer cardComparer)
    {
        _logger = logger;
        _options = options.Value;
        _dbMiddleware = dbMiddleware;
        _cardComparer = cardComparer;
    }

    public async Task<IActionResult> Index()
    {
        var cards = await _dbMiddleware.GetAllCards();
        var cardsOrdered = cards.OrderBy(c => c, _cardComparer);
        var result = new CardPriorityModel(cardsOrdered, _options);
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

