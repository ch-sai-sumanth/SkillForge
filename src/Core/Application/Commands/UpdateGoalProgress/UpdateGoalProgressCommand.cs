using MediatR;

namespace Application.Commands.UpdateGoalProgress;

public class UpdateGoalProgressCommand : IRequest<Unit>
{
    public string GoalId { get; set; } = null!;
    public int ProgressPercentage { get; set; }
    public string? UpdateReason { get; set; }
}