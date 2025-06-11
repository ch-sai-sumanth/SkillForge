using User.Domain.Entities;

namespace Domain.Repositories;

public interface ITaskRepository
{
    Task<List<LearningTask>> GetTasksAssignedByMentorAsync(string mentorId);
    Task<LearningTask?> GetTaskByIdAsync(string taskId);
    Task UpdateTaskAsync(LearningTask task);
    Task DeleteTaskAsync(string taskId);
    Task<List<LearningTask>> GetTasksByMenteeAsync(string menteeId);
    Task CreateTaskAsync(LearningTask task);
    Task<LearningTask?> GetByIdAsync(string taskId);
    Task UpdateAsync(LearningTask task);
    Task DeleteAsync(LearningTask task);
}