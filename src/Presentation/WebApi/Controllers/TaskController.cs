using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController :ControllerBase
{
    
    
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignTask([FromBody] AssignTaskDto dto)
    {
        await _taskService.AssignTaskAsync(dto);
        return Ok("Task assigned.");
    }
    
    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTaskById(string taskId)
    {
        var task = await _taskService.GetTaskByIdAsync(taskId);
        if (task == null)
            return NotFound("Task not found.");

        return Ok(task);
    }

    [HttpGet("mentee/{menteeId}")]
    public async Task<IActionResult> GetTasksForMentee(string menteeId)
    {
        var tasks = await _taskService.GetTasksByMenteeIdAsync(menteeId);
        return Ok(tasks);
    }

    [HttpPut("{taskId}/status")]
    public async Task<IActionResult> UpdateTaskStatus(string taskId, [FromBody] UpdateTaskStatusDto dto)
    {
        await _taskService.UpdateTaskStatusAsync(taskId, dto.Status);
        return Ok("Task status updated.");
    }
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(string taskId, [FromBody] UpdateTaskDto dto)
    {
        await _taskService.UpdateTaskAsync(taskId, dto);
        return Ok("Task updated.");
    }


    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(string taskId)
    {
        await _taskService.DeleteTaskAsync(taskId);
        return Ok("Task deleted.");
    }
}