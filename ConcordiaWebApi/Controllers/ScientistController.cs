namespace ConcordiaWebApi.Controllers;

using ConcordiaLib.Abstract;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]

public class ScientistController : ControllerBase
{
    private readonly IDbMiddleware _dbMiddleware;

    public ScientistController(IDbMiddleware dbMiddleware)
    {
        _dbMiddleware = dbMiddleware;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllScientist()
    {
        return Ok(await _dbMiddleware.GetAllPeople());
    }

    [HttpGet]
    public async Task<IActionResult> GetScientistAssignments(string scientistId)
    {
        var cards = await _dbMiddleware.GetScientistAssignments(scientistId);
        var result = cards.OrderByDescending(c => c.Priority);
        return Ok(result);
    }
}
