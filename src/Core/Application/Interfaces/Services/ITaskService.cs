using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface ITaskService
{
    Task AssignTaskAsync(AssignTaskDto dto);
    Task<List<TaskDto>> GetTasksByMenteeIdAsync(string menteeId);
    Task UpdateTaskStatusAsync(string taskId, string dtoStatus);
    Task<TaskDto?> GetTaskByIdAsync(string taskId);
    Task UpdateTaskAsync(string taskId, UpdateTaskDto dto);
    Task DeleteTaskAsync(string taskId);
}