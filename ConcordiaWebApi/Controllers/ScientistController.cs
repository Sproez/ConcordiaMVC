namespace ConcordiaWebApi.Controllers;

using Options;
using Microsoft.Extensions.Options;
using ConcordiaLib.Abstract;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using ConcordiaLib.Utils;

[ApiController]
[Route("[controller]")]

public class ScientistController : ControllerBase
{
    private readonly IDbMiddleware _dbMiddleware;
    private readonly WebApiOptions _options;
    private readonly CardComparer _cardComparer;

    public ScientistController(IOptions<WebApiOptions> options, IDbMiddleware dbMiddleware)
    {
        _options = options.Value;
        _dbMiddleware = dbMiddleware;

        _cardComparer = new CardComparer(_options.CompletedListId);
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllScientist()
    {
        var people = await _dbMiddleware.GetAllPeople();
        var result = people.Select(p => new PersonDto(p));
        return Ok(result);
    }

    [HttpGet("{scientistId}/Assignments")]
    public async Task<IActionResult> GetScientistAssignments(string scientistId)
    {
        try
        {
            var cards = await _dbMiddleware.GetScientistAssignments(scientistId);
            var cardsOrdered = cards.OrderBy(c => c, _cardComparer);
            var result = cardsOrdered.Select(c => new CardDto(c)).ToList();
            return Ok(result);
        }
        catch (Exception)
        {
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{scientistId}/PerformanceReport")]
    public async Task<IActionResult> GetPerformanceReport(string scientistId)
    {
        try
        {
            /*
            var cards = await _dbMiddleware.GetScientistAssignments(scientistId);
            int completed = cards.Count(c => c.CardListId == _options.CompletedListId);
            int total = cards.Count();
            */
            (int completed, int total) = await _dbMiddleware.GetScientistPerformanceReport(scientistId, _options.CompletedListId);
            string result = $"Completed : {completed} / Total : {total}";

            return Ok(result);
        }
        catch (Exception)
        {
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
