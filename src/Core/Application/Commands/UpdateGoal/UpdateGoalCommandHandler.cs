using Application.Interfaces;
using MediatR;

namespace Application.Commands.UpdateGoal;

public class UpdateGoalCommandHandler : IRequestHandler<UpdateGoalCommand, string>
{
    private readonly IGoalService _goalService;

    public UpdateGoalCommandHandler(IGoalService goalService)
    {
        _goalService = goalService;
    }
    public async Task<string> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
        await _goalService.UpdateGoalAsync(request.GoalId, request.UpdatedGoal);
        return "Goal progress updated successfully.";
    }
}