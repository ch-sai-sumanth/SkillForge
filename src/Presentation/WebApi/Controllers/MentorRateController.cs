using Application.Commands.DeleteMentorRate;
using Application.Commands.SetMentorRate;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.GetMentorRate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MentorRateController : ControllerBase
{
    private readonly IMediator _mediator;

    public MentorRateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("set")]
    public async Task<IActionResult> SetRate([FromBody] MentorRateDto dto)
    {
        var command = new SetMentorRateCommand { MentorRate = dto };
        var result = await _mediator.Send(command);
        return result
            ? Ok(new { message = "Mentor rate saved successfully." })
            : BadRequest("Failed to save mentor rate.");
    }

    [HttpGet("{mentorId}")]
    public async Task<IActionResult> GetRate(string mentorId)
    {
        var query = new GetMentorRateQuery { MentorId = mentorId };
        var rate = await _mediator.Send(query);

        return rate != null
            ? Ok(rate)
            : NotFound(new { message = "Mentor rate not found." });
    }

    [HttpDelete("{mentorId}")]
    public async Task<IActionResult> DeleteRate(string mentorId)
    {
        var command = new DeleteMentorRateCommand { MentorId = mentorId };
        var deleted = await _mediator.Send(command);

        return deleted
            ? Ok(new { message = "Mentor rate deleted successfully." })
            : NotFound(new { message = "Mentor rate not found." });
    }
}
