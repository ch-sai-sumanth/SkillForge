using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/sessions")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    // ✅ Constructor name matches the class
    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }


    [HttpPost("book")]
    public async Task<IActionResult> BookSession([FromBody] BookSessionDto bookSessionDto)
    {
        var session = await _sessionService.BookSessionAsync(bookSessionDto);
        return Ok(session);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserSessions(string userId)
    {
        var sessions = await _sessionService.GetUserSessionsAsync(userId);
        return Ok(sessions);
    }

    [HttpGet("{sessionId}")]
    public async Task<IActionResult> GetSession([FromRoute] string sessionId)
    {
        var session = await _sessionService.GetSessionByIdAsync(sessionId);
        if (session == null)
            return NotFound("Session not found.");
        return Ok(session);
    }

    [HttpPost("cancel/{id}")]
    public async Task<IActionResult> CancelSession(string id)
    {
        await _sessionService.CancelSessionAsync(id);
        return Ok("Session cancelled.");
    }

    [HttpPut("{sessionId}")]
    public async Task<IActionResult> UpdateSession([FromRoute] string sessionId, [FromBody] UpdateSessionDto updateSessionDto)
    {
        await _sessionService.UpdateSessionAsync(sessionId, updateSessionDto);
        return Ok("Session updated.");
    }


    [HttpPost("{sessionId}/accept")]
    public async Task<IActionResult> AcceptSession(string sessionId)
    {
        await _sessionService.AcceptSessionAsync(sessionId);
        return Ok("Session accepted.");
    }
    
    [HttpPost("{sessionId}/decline")]
    public async Task<IActionResult> DeclineSession(string sessionId)
    {
        await _sessionService.DeclineSessionAsync(sessionId);
        return Ok("Session declined.");
    }
    
    [HttpGet("mentor/{mentorId}/pending")]
    public async Task<IActionResult> GetPendingSessions(string mentorId)
    {
        var sessions = await _sessionService.GetPendingSessionsForMentorAsync(mentorId);
        return Ok(sessions);
    }
    
}
