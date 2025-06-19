using Application.DTOs;
using MediatR;

namespace Application.Commands.CreateGoal;

public class CreateGoalCommand : IRequest<string>
{
    public CreateGoalDto Goal { get; set; } = null;
}