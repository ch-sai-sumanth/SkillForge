using Application.DTOs;
using Application.Interfaces;
using MediatR;
using User.Domain.Entities;

namespace Application.Queries.GetGoals;

public class GetGoalByMenteeIdQueryHandler : IRequestHandler<GetGoalsByMenteeIdQuery, List<LearningGoal>>
{
    private readonly IGoalService _goalService;

    public GetGoalByMenteeIdQueryHandler(IGoalService goalService)
    {
        _goalService = goalService;
    }
    public async Task<List<LearningGoal>> Handle(GetGoalsByMenteeIdQuery request, CancellationToken cancellationToken)
    {
        return await _goalService.GetGoalsByMenteeIdAsync(request.MenteeId);
    }
}