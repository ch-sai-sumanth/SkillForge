using Application.Interfaces;
using Application.Queries.AvailableMentors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace API.Controllers;

[ApiController]
[Route("api/mentees")]
public class MenteeController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenteeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("available-mentors")]
    public async Task<IActionResult> GetAvailableMentors([FromQuery] string skill, [FromQuery] string dateTime)
    {
        if (!DateTime.TryParse(dateTime, out var parsedDateTime))
            return BadRequest("Invalid datetime format. Use ISO format like '2025-06-12T14:30:00'.");

        var query = new GetAvailableMentorsQuery
        {
            Skill = skill,
            DateTime = parsedDateTime
        };

        var results = await _mediator.Send(query);
        return Ok(results);
    }
}