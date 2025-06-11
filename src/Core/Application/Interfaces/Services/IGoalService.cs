using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface IGoalService
{
    Task CreateGoalAsync(CreateGoalDto dto);
    Task<List<LearningGoal>> GetGoalsByMenteeIdAsync(string menteeId);
    Task UpdateGoalAsync(string goalId,UpdateGoalDto dto);
    Task DeleteGoalAsync(string goalId);
}