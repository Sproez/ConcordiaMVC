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
}

