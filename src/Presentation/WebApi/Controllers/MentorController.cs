using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/mentors")]
public class MentorController : ControllerBase
{
    private readonly IUserService _userService;

    public MentorController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("{mentorId}/availability")]
    public async Task<IActionResult> SetAvailability(string mentorId, [FromBody] List<AvailabilityDto> availabilityDtos)
    {
        await _userService.SetMentorAvailabilityAsync(mentorId, availabilityDtos);
        return Ok(new { message = "Availability updated successfully." });
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchMentors([FromQuery] string skill, [FromQuery] DayOfWeek day, [FromQuery] string time)
    {
        if (!TimeSpan.TryParse(time, out var parsedTime))
            return BadRequest("Invalid time format. Use HH:mm (e.g. 14:30).");

        var results = await _userService.SearchMentorsBySkillAndAvailabilityAsync(skill, day, parsedTime);
        return Ok(results);
    }
}