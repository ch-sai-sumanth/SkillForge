using Application.Commands.Task.AssignTask;
using Application.Commands.Task.DeleteTask;
using Application.Commands.Task.UpdateTask;
using Application.Commands.Task.UpdateTaskStatus;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.Task.GetTask;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Mentor")]
    [HttpPost("assign")]
    public async Task<IActionResult> AssignTask([FromBody] AssignTaskDto dto)
    {
        await _mediator.Send(new AssignTaskCommand { Dto = dto });
        return Ok("Task assigned.");
    }

    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTaskById(string taskId)
    {
        var task = await _mediator.Send(new GetTaskByIdQuery(taskId));
        return task == null ? NotFound("Task not found.") : Ok(task);
    }

    [HttpGet("mentee/{menteeId}")]
    public async Task<IActionResult> GetTasksForMentee(string menteeId)
    {
        var tasks = await _mediator.Send(new GetTasksForMenteeQuery(menteeId));
        return Ok(tasks);
    }

    [HttpPut("{taskId}/status")]
    public async Task<IActionResult> UpdateTaskStatus(string taskId, [FromBody] UpdateTaskStatusDto dto)
    {
        await _mediator.Send(new UpdateTaskStatusCommand { TaskId = taskId, Status = dto.Status });
        return Ok("Task status updated.");
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(string taskId, [FromBody] UpdateTaskDto dto)
    {
        await _mediator.Send(new UpdateTaskCommand { TaskId = taskId, Dto = dto });
        return Ok("Task updated.");
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(string taskId)
    {
        await _mediator.Send(new DeleteTaskCommand { TaskId = taskId });
        return Ok("Task deleted.");
    }
}
