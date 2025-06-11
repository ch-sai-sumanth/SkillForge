using Domain.Repositories;
using MongoDB.Driver;
using User.Domain.Entities;

namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IMongoCollection<LearningTask> _tasks;

    public TaskRepository(IMongoDatabase database)
    {
        _tasks = database.GetCollection<LearningTask>("LearningTasks");
    }

    public async Task CreateTaskAsync(LearningTask task)
    {
        await _tasks.InsertOneAsync(task);
    }

    public Task<LearningTask?> GetByIdAsync(string taskId)
    {
        return _tasks.Find(t => t.Id == taskId).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(LearningTask task)
    {
        await _tasks.ReplaceOneAsync(t => t.Id == task.Id, task);
    }

    public async Task DeleteAsync(LearningTask task)
    {
        await _tasks.DeleteOneAsync(t => t.Id == task.Id);
    }

    public async Task<List<LearningTask>> GetTasksByMenteeAsync(string menteeId)
    {
        return await _tasks.Find(t => t.MenteeId == menteeId).ToListAsync();
    }

    public async Task<List<LearningTask>> GetTasksAssignedByMentorAsync(string mentorId)
    {
        return await _tasks.Find(t => t.MentorId == mentorId).ToListAsync();
    }

    public async Task<LearningTask?> GetTaskByIdAsync(string taskId)
    {
        return await _tasks.Find(t => t.Id == taskId).FirstOrDefaultAsync();
    }

    public async Task UpdateTaskAsync(LearningTask task)
    {
        await _tasks.ReplaceOneAsync(t => t.Id == task.Id, task);
    }

    public async Task DeleteTaskAsync(string taskId)
    {
        await _tasks.DeleteOneAsync(t => t.Id == taskId);
    }
}