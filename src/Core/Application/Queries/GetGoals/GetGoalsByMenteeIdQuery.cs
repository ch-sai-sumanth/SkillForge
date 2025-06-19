using Application.DTOs;
using MediatR;
using User.Domain.Entities;

namespace Application.Queries.GetGoals;

public class GetGoalsByMenteeIdQuery:IRequest<List<LearningGoal>>
{
    public string MenteeId { get; set; }
}