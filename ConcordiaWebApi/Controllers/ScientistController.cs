﻿using ConcordiaWebApi.Options;

namespace ConcordiaWebApi.Controllers;

using ConcordiaLib.Abstract;
using Microsoft.AspNetCore.Mvc;
using Dtos;

[ApiController]
[Route("[controller]")]

public class ScientistController : ControllerBase
{
    private readonly IDbMiddleware _dbMiddleware;
    private readonly WebApiOptions _options = new WebApiOptions();

    public ScientistController(IDbMiddleware dbMiddleware)
    {
        _dbMiddleware = dbMiddleware;
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
        var cards = await _dbMiddleware.GetScientistAssignments(scientistId);
        var result = cards.Select(c => new CardDto(c)).ToList();
        result.Sort();
        return Ok(result);
    }

    [HttpGet("{scientistId}/PerformanceReport")]
    public async Task<IActionResult> GetPerformanceReport(string scientistId)
    {
        var cards = await _dbMiddleware.GetScientistAssignments(scientistId);
        int completed = cards.Count(c => c.CardListId == _options.CompletedListId);
        int total = cards.Count();
        var result = cards.Select(c => new CardDto(c)).ToList();

        return Ok(result);
    }
}
