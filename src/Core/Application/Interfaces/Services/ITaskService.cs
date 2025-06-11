using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface ITaskService
{
    Task AssignTaskAsync(AssignTaskDto dto);
    Task<List<LearningTask>> GetTasksByMenteeIdAsync(string menteeId);
    Task UpdateTaskStatusAsync(string taskId, string dtoStatus);
    Task<LearningTask?> GetTaskByIdAsync(string taskId);
    Task UpdateTaskAsync(string taskId, UpdateTaskDto dto);
    Task DeleteTaskAsync(string taskId);
}