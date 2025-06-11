using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/mentee")]
public class GoalController : ControllerBase
{
    private readonly IGoalService _goalService;

    public GoalController(IGoalService goalService)
    {
        _goalService = goalService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalDto dto)
    {
        await _goalService.CreateGoalAsync(dto);
        return Ok("Goal created.");
    }
    
    [HttpPut("{menteeId}/goals/{goalId}/progress")]
    public async Task<IActionResult> UpdateGoalProgress(string goalId, [FromBody] UpdateGoalDto dto)
    {
        await _goalService.UpdateGoalAsync(goalId,dto);
        return Ok("Goal progress updated.");
    }

    [HttpGet("{menteeId}/goals")]
    public async Task<IActionResult> GetGoalsForMentee(string menteeId)
    {
        var goals = await _goalService.GetGoalsByMenteeIdAsync(menteeId);
        return Ok(goals);
    }
    
    [HttpDelete]
    [Route("{menteeId}/goals/{goalId}")]
    public async Task<IActionResult> DeleteGoal(string goalId)
    {
        await _goalService.DeleteGoalAsync(goalId);
        return Ok("Goal deleted.");
    }
}