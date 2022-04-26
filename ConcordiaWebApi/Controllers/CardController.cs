using ConcordiaLib.Domain;
using ConcordiaWebApi.Dtos;
using ConcordiaWebApi.Options;
using Microsoft.Extensions.Options;

namespace ConcordiaWebApi.Controllers;

using ConcordiaLib.Abstract;
using Microsoft.AspNetCore.Mvc;
using Dtos;

[ApiController]
[Route("[controller]")]

public class CardController : ControllerBase
{
    private readonly IDbMiddleware _dbMiddleware;
    private readonly WebApiOptions _options;

    public CardController(IOptions<WebApiOptions> options, IDbMiddleware dbMiddleware)
    {
        _options = options.Value;
        _dbMiddleware = dbMiddleware;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllCards()
    {
        var cards = await _dbMiddleware.GetAllCards();
        var result = cards.Select(c => new CardDto(c)).ToList();
        result.Sort();
        return Ok(result);
    }

    [HttpPost("Comment")]
    public async Task<IActionResult> PostComment([FromBody] CommentDto cDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newComment = new Comment(cDto.Text, DateTime.UtcNow) { Id = cDto.Id, CardId = cDto.CardId, PersonId = cDto.PersonId };
            await _dbMiddleware.PostComment(newComment);

            return Ok();
        }
        catch (Exception)
        {
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("AllLists")]
    public async Task<IActionResult> GetAllLists()
    {
        var cardLists = await _dbMiddleware.GetAllCardLists();
        var result = cardLists.Select(c => new CardListDto(c));
        return Ok(result);
    }

    [HttpPost("{id}/ChangeList")]
    public async Task<IActionResult> ChangeList(string id, [FromBody] string listId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _dbMiddleware.ChangeCardStatus(id, listId);
            return Ok();
        }
        catch (Exception e)
        {
            string test = e.Message;
            return Problem(statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
