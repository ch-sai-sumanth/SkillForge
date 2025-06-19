using Application.DTOs;
using MediatR;

namespace Application.Commands.UpdateGoal;

public class UpdateGoalCommand:IRequest<string>
{
    public string GoalId { get; set; } = string.Empty;
    public UpdateGoalDto UpdatedGoal { get; set; } = null!;
}