using Application.Commands.MentorAvailability;
using Application.Commands.UpdateMentorSkills;
using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace API.Controllers;

[ApiController]
[Route("api/mentors")]
public class MentorController : ControllerBase
{
    private readonly IMediator _mediator;

    public MentorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{mentorId}/availability")]
    public async Task<IActionResult> SetAvailability([FromRoute] string mentorId, [FromBody] List<AvailabilityDto> availabilityDtos)
    {
        var command = new SetMentorAvailabilityCommand
        {
            MentorId = mentorId,
            Availabilities = availabilityDtos
        };

        var result = await _mediator.Send(command);
        return result
            ? Ok(new { message = "Availability updated successfully." })
            : BadRequest("Failed to update availability.");
    }

    [HttpPut("{mentorId}/skills")]
    public async Task<IActionResult> UpdateSkills([FromRoute] string mentorId, [FromBody] UpdateSkillsDto dto)
    {
        var command = new UpdateMentorSkillsCommand
        {
            MentorId = mentorId,
            Skills = dto.Skills
        };

        var result = await _mediator.Send(command);
        return result
            ? Ok(new { message = "Skills updated successfully." })
            : NotFound("Mentor not found.");
    }
}
