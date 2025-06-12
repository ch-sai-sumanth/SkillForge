using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MentorRateController : ControllerBase
{
    private readonly IMentorRateService _mentorRateService;

    public MentorRateController(IMentorRateService mentorRateService)
    {
        _mentorRateService = mentorRateService;
    }
    

    // POST: api/mentorRate/set
    [HttpPost("set")]
    public async Task<IActionResult> SetRate([FromBody] MentorRateDto dto)
    {
        await _mentorRateService.SetMentorRateAsync(dto);
        return Ok(new { message = "Mentor rate saved successfully." });
    }

    // GET: api/mentorRate/{mentorId}
    [HttpGet("{mentorId}")]
    public async Task<IActionResult> GetRate(string mentorId)
    {
        var rate = await _mentorRateService.GetMentorRateAsync(mentorId);
        if (rate == null)
            return NotFound(new { message = "Mentor rate not found." });

        return Ok(rate);
    }

    // DELETE: api/mentorRate/{mentorId}
    [HttpDelete("{mentorId}")]
    public async Task<IActionResult> DeleteRate(string mentorId)
    {
        var deleted = await _mentorRateService.DeleteMentorRateAsync(mentorId);
        if (!deleted)
            return NotFound(new { message = "Mentor rate not found." });

        return Ok(new { message = "Mentor rate deleted successfully." });
    }
}
