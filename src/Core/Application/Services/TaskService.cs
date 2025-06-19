using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Repositories;
using User.Domain.Entities;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _modelMapper;

    public TaskService(ITaskRepository taskRepository,IMapper modelMapper)
    {
        _taskRepository = taskRepository;
        _modelMapper = modelMapper;
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

    public async Task<List<TaskDto>> GetTasksByMenteeIdAsync(string menteeId)
    {
       List<LearningTask> tasks = await _taskRepository.GetTasksByMenteeAsync(menteeId);
         return _modelMapper.Map<List<TaskDto>>(tasks);
    }
    public async Task<TaskDto?> GetTaskByIdAsync(string taskId)
    {
        var task= await _taskRepository.GetByIdAsync(taskId);
        
        return _modelMapper.Map<TaskDto>(task);
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