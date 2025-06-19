using Application.Interfaces;
using MediatR;

namespace Application.Commands.CreateGoal;

public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, string>
{
    private readonly IGoalService _goalService;

    public CreateGoalCommandHandler(IGoalService goalService)
    {
        _goalService = goalService;
    }
    public async Task<string> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        await _goalService.CreateGoalAsync(request.Goal);
        return "Goal created successfully.";
    }
}