using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchingController : ControllerBase
{
    private readonly IUserService _userService;

    public MatchingController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("mentors-by-skills")]
    public async Task<IActionResult> GetMentorsBySkills([FromBody] List<string> skills)
    {
        if (skills == null || skills.Count == 0)
            return BadRequest("Skills list cannot be empty.");

        var matches = await _userService.GetMentorsBySkillsWithScoreAsync(skills);
        return Ok(matches);
    }
}