using System.Threading.Tasks;
using Application.Commands.CreateGoal;
using Application.Commands.DeleteGoal;
using Application.Commands.UpdateGoal;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.GetGoals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/mentee")]
public class GoalController : ControllerBase
{
    private readonly IMediator _mediator;


    public GoalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalDto dto)
    {
        
        var result = await _mediator.Send(new CreateGoalCommand
        {
           Goal = dto
        });
        return Ok(result);
    }
    
    [HttpPut("{menteeId}/goals/{goalId}/progress")]
    public async Task<IActionResult> UpdateGoalProgress(string goalId, [FromBody] UpdateGoalDto updateGoalDto)
    {
      var result = await _mediator.Send(new UpdateGoalCommand
      {
          GoalId = goalId,
          UpdatedGoal = updateGoalDto
      });
        return Ok(result);
    }
    
    [HttpGet("{menteeId}/goals")]
    public async Task<IActionResult> GetGoalsForMentee(string menteeId)
    {
        var goals = await _mediator.Send(new GetGoalsByMenteeIdQuery()
        {
            MenteeId = menteeId
        });
        return Ok(goals);
    }
    
    [HttpDelete]
    [Route("{menteeId}/goals/{goalId}")]
    public async Task<IActionResult> DeleteGoal(string goalId)
    {
        var result = await _mediator.Send(new DeleteGoalCommand
        {
            GoalId = goalId
        });
        return Ok(result);
    }
}