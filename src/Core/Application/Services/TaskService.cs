using Application.DTOs;
using Application.Interfaces;
using Domain.Repositories;
using User.Domain.Entities;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task AssignTaskAsync(AssignTaskDto dto)
    {
        var task = new LearningTask
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            MenteeId = dto.MenteeId,
            MentorId = dto.MentorId,
            Status = "Assigned"
        };

        await _taskRepository.CreateTaskAsync(task);
    }

    public async Task<List<LearningTask>> GetTasksByMenteeIdAsync(string menteeId)
    {
        return await _taskRepository.GetTasksByMenteeAsync(menteeId);
    }
    public async Task<LearningTask?> GetTaskByIdAsync(string taskId)
    {
        return await _taskRepository.GetByIdAsync(taskId);
    }

    public async Task UpdateTaskStatusAsync(string taskId, string newStatus)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        if (task == null)
            throw new InvalidOperationException("Task not found.");
        
        task.Id= taskId;
        task.Status = newStatus;
        await _taskRepository.UpdateTaskAsync(task);
    }
    public async Task UpdateTaskAsync(string taskId, UpdateTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            throw new InvalidOperationException("Task not found.");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;

        await _taskRepository.UpdateAsync(task);
    }

    // ✅ Delete a task
    public async Task DeleteTaskAsync(string taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            throw new InvalidOperationException("Task not found.");

        await _taskRepository.DeleteAsync(task);
    }
}