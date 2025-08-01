using MediatR;
using Domain.ValueObjects;
using Domain.Common;
using Application.Interfaces.Repositories;

namespace Application.Commands.UpdateGoalProgress;

public class UpdateGoalProgressCommandHandler : IRequestHandler<UpdateGoalProgressCommand, Unit>
{
    private readonly IGoalRepository _goalRepository;

    public UpdateGoalProgressCommandHandler(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task<Unit> Handle(UpdateGoalProgressCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the goal
            var goalId = LearningGoalId.Create(request.GoalId);
            var learningGoal = await _goalRepository.GetByIdAsync(goalId);
            
            if (learningGoal == null)
                throw new InvalidOperationException($"Learning goal with ID {request.GoalId} not found");

            // Use domain method to update progress
            learningGoal.UpdateProgress(request.ProgressPercentage, request.UpdateReason);

            // Save changes (domain events will be dispatched by infrastructure)
            await _goalRepository.UpdateAsync(learningGoal);

            return Unit.Value;
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException($"Invalid progress data: {ex.Message}", ex);
        }
        catch (DomainException ex)
        {
            throw new InvalidOperationException($"Cannot update progress: {ex.Message}", ex);
        }
    }
}