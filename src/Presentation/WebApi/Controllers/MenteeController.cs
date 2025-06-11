using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/mentees")]
public class MenteeController : ControllerBase
{
    private readonly IUserService _userService;

    public MenteeController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("available-mentors")]
    public async Task<IActionResult> GetAvailableMentors([FromQuery] string skill, [FromQuery] string dateTime)
    {
        if (!DateTime.TryParse(dateTime, out var parsedDateTime))
            return BadRequest("Invalid datetime format. Use ISO format like '2025-06-12T14:30:00'.");

        var results = await _userService.SearchMentorsBySkillAndAvailabilityAsync(skill, parsedDateTime);
        return Ok(results);
    }
}