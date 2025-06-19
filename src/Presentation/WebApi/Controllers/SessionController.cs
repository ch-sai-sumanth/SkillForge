using Application.Commands.sessions;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.GetSession;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
using System.Threading.Tasks;
[ApiController]
[Route("api/sessions")]
[Authorize]
public class SessionController : ControllerBase
{
    private readonly IMediator _mediator;
    public SessionController(IMediator mediator) => _mediator = mediator;

    [HttpPost("book")]
    public async Task<IActionResult> BookSession([FromBody] BookSessionDto dto)
    {
        var result = await _mediator.Send(new BookSessionCommand(dto));
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserSessions(string userId)
    {
        var result = await _mediator.Send(new GetUserSessionsQuery(userId));
        return Ok(result);
    }

    [HttpGet("{sessionId}")]
    public async Task<IActionResult> GetSession(string sessionId)
    {
        var result = await _mediator.Send(new GetSessionByIdQuery(sessionId));
        return result != null ? Ok(result) : NotFound("Session not found.");
    }

    [HttpPost("cancel/{id}")]
    public async Task<IActionResult> CancelSession(string id)
    {
        await _mediator.Send(new CancelSessionCommand(id));
        return Ok("Session cancelled.");
    }

    [HttpPut("{sessionId}")]
    public async Task<IActionResult> UpdateSession(string sessionId, [FromBody] UpdateSessionDto dto)
    {
        await _mediator.Send(new UpdateSessionCommand(sessionId, dto));
        return Ok("Session updated.");
    }

    [HttpPost("{sessionId}/accept")]
    public async Task<IActionResult> AcceptSession(string sessionId)
    {
        await _mediator.Send(new AcceptSessionCommand(sessionId));
        return Ok("Session accepted.");
    }

    [HttpPost("{sessionId}/decline")]
    public async Task<IActionResult> DeclineSession(string sessionId)
    {
        await _mediator.Send(new DeclineSessionCommand(sessionId));
        return Ok("Session declined.");
    }

    [HttpGet("mentor/{mentorId}/pending")]
    public async Task<IActionResult> GetPendingSessions(string mentorId)
    {
        var result = await _mediator.Send(new GetPendingSessionsForMentorQuery(mentorId));
        return Ok(result);
    }

    [HttpPost("{sessionId}/complete")]
    public async Task<IActionResult> CompleteSession(string sessionId)
    {
        await _mediator.Send(new CompleteSessionCommand(sessionId));
        return Ok("Session marked as completed.");
    }

    [HttpGet("{sessionId}/history")]
    public async Task<IActionResult> GetSessionHistory(string sessionId)
    {
        var result = await _mediator.Send(new GetSessionHistoryQuery(sessionId));
        return Ok(result);
    }
}
