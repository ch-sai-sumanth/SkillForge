using Application.Interfaces;
using MediatR;

namespace Application.Commands.DeleteGoal;

public class DeleteGoalCommandHandler: IRequestHandler<DeleteGoalCommand, string>
{
    private readonly IGoalService _goalService;

    public DeleteGoalCommandHandler(IGoalService goalService)
    {
        _goalService = goalService;
    }
    public async Task<string> Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
        await _goalService.DeleteGoalAsync(request.GoalId);
        return "Goal deleted successfully.";
    }
}