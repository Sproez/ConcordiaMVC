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
        return View();
    }

   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
