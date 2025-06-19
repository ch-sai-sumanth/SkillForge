using Application.DTOs;
using MediatR;

namespace Application.Queries.GetMentorSkills;

public class GetMentorsBySkillsQuery : IRequest<List<MentorMatchDto>>
{
    public List<string> Skills { get; set; } = new();
}