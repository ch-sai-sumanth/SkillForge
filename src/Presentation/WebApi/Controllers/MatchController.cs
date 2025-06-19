using Application.Interfaces;
using System.Threading.Tasks;
using Application.Queries.GetMentorSkills;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchingController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatchingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("mentors-by-skills")]
    public async Task<IActionResult> GetMentorsBySkills([FromBody] List<string> skills)
    {
        if (skills == null || skills.Count == 0)
            return BadRequest("Skills list cannot be empty.");

        var result = await _mediator.Send(new GetMentorsBySkillsQuery { Skills = skills });
        return Ok(result);
    }
}