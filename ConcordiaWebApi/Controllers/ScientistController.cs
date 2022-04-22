namespace ConcordiaWebApi.Controllers;

using ConcordiaLib.Abstract;
using Microsoft.AspNetCore.Mvc;
using Dtos;

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
        var people = await _dbMiddleware.GetAllPeople();
        var result = people.Select(p => new PersonDto(p));
        return Ok(result);
    }

    [HttpGet("{scientistId}")]
    public async Task<IActionResult> GetScientistAssignments(string scientistId)
    {
        var cards = await _dbMiddleware.GetScientistAssignments(scientistId);
        var result = cards.Select(c => new CardDto(c)).OrderByDescending(c => c.Priority);
        return Ok(result);
    }
}
