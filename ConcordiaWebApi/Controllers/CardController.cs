﻿using ConcordiaLib.Domain;
using ConcordiaWebApi.Dtos;

namespace ConcordiaWebApi.Controllers;

using ConcordiaLib.Abstract;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]

public class CardController : ControllerBase
{
    private readonly IDbMiddleware _dbMiddleware;

    public CardController(IDbMiddleware dbMiddleware)
    {
        _dbMiddleware = dbMiddleware;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCards()
    {
        var cards = await _dbMiddleware.GetAllCards();
        var result = cards.OrderByDescending(c => c.Priority);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostComment([FromBody] CommentDto cDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newComment = new Comment(cDto.Text, DateTime.Now) { CardId = cDto.CardId, PersonId = cDto.PersonId };
            await _dbMiddleware.PostComment(newComment);

            return Ok();
        }
        catch (Exception e)
        {
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
