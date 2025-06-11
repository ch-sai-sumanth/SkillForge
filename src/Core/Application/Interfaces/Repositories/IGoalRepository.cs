using User.Domain.Entities;

namespace Domain.Repositories;

public interface IGoalRepository
{
    Task CreateGoalAsync(LearningGoal goal);
    Task<List<LearningGoal>> GetGoalsByMenteeIdAsync(string menteeId);
    Task<LearningGoal?> GetGoalByIdAsync(string goalId);
    Task UpdateGoalAsync(LearningGoal goal);
    Task DeleteGoalAsync(string goalId);
}