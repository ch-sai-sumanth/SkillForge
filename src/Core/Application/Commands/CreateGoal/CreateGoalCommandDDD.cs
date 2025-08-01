using MediatR;
using Application.DTOs;

namespace Application.Commands.CreateGoal;

public class CreateGoalCommandDDD : IRequest<GoalDto>
{
    public string MenteeId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime TargetDate { get; set; }
}