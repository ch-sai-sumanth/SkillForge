using Application.DTOs;
using MediatR;

namespace Application.Queries.AvailableMentors;

public class GetAvailableMentorsQuery : IRequest<List<MentorMatchDto>>
{
    public string Skill { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
}