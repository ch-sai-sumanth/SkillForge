using MediatR;

namespace Application.Commands.DeleteGoal;

public class DeleteGoalCommand:IRequest<string>
{
    public string GoalId { get; set; }=String.Empty;
}