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
    public async Task<IActionResult> SetAvailability([FromRoute] string mentorId, [FromBody] List<AvailabilityDto> availabilityDtos)
    {
        await _userService.SetMentorAvailabilityAsync(mentorId, availabilityDtos);
        return Ok(new { message = "Availability updated successfully." });
    }
    
    
    [HttpPut("{mentorId}/skills")]
    public async Task<IActionResult> UpdateSkills([FromRoute] string mentorId, [FromBody] UpdateSkillsDto dto)
    {
        var updated = await _userService.UpdateMentorSkillsAsync(mentorId, dto.Skills);
        return updated ? Ok(new { message = "Skills updated successfully." }) : NotFound("Mentor not found.");
    }
}