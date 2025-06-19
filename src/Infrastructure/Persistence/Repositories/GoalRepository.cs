using Application.DTOs;
using Domain.Repositories;
using MongoDB.Driver;
using User.Domain.Entities;

namespace Infrastructure.Repositories;

public class GoalRepository : IGoalRepository
{
    private readonly IMongoCollection<LearningGoal> _goals;

    public GoalRepository(IMongoDatabase database)
    {
        _goals = database.GetCollection<LearningGoal>("LearningGoals");
    }

    public async Task CreateGoalAsync(LearningGoal goal)
    {
        await _goals.InsertOneAsync(goal);
    }
    
    public async Task<List<LearningGoal>> GetGoalsByMenteeIdAsync(string menteeId)
    {
        return await _goals.Find(g => g.MenteeId == menteeId).ToListAsync();
    }

    public async Task<LearningGoal?> GetGoalByIdAsync(string goalId)
    {
        return await _goals.Find(g => g.Id == goalId).FirstOrDefaultAsync();
    }

    public async Task UpdateGoalAsync(LearningGoal goal)
    {
        await _goals.ReplaceOneAsync(g => g.Id == goal.Id, goal);
    }

    public async Task DeleteGoalAsync(string goalId)
    {
        await _goals.DeleteOneAsync(g => g.Id == goalId);
    }
}