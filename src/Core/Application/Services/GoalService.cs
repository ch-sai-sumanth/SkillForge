using Application.DTOs;
using Application.Interfaces;
using Domain.Repositories;
using User.Domain.Entities;

namespace Application.Services;

public class GoalService : IGoalService
{
    private readonly IGoalRepository _goalRepository;

    public GoalService(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task CreateGoalAsync(CreateGoalDto dto)
    {
        var goal = new LearningGoal
        {
            MenteeId = dto.MenteeId,
            Title = dto.Title,
            Description = dto.Description,
            TargetDate = dto.TargetDate,
            Status = "Pending"
        };

        await _goalRepository.CreateGoalAsync(goal);
    }

    public async Task<List<LearningGoal>> GetGoalsByMenteeIdAsync(string menteeId)
    {
        return await _goalRepository.GetGoalsByMenteeIdAsync(menteeId);
    }

    public async Task UpdateGoalAsync(string goalId,UpdateGoalDto dto)
    {
        var goal = await _goalRepository.GetGoalByIdAsync(goalId);
        if (goal == null)
            throw new InvalidOperationException("Goal not found.");

        if (dto.ProgressPercentage is not null)
        {
            if (dto.ProgressPercentage < 0 || dto.ProgressPercentage > 100)
                throw new ArgumentOutOfRangeException(nameof(dto.ProgressPercentage), "Progress must be between 0 and 100.");

            goal.ProgressPercentage = dto.ProgressPercentage.Value;
        }

        if (!string.IsNullOrWhiteSpace(dto.Status))
        {
            var allowedStatuses = new[] { "Pending", "InProgress", "Completed" };
            if (!allowedStatuses.Contains(dto.Status))
                throw new ArgumentException("Invalid status value.", nameof(dto.Status));

            goal.Status = dto.Status;
        }

        await _goalRepository.UpdateGoalAsync(goal);
    }

    public async Task DeleteGoalAsync(string goalId)
    {
        await _goalRepository.DeleteGoalAsync(goalId);
    }
}