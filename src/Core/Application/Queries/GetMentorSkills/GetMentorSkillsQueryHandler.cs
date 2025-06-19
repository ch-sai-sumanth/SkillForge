using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetMentorSkills;

public class GetMentorsBySkillsQueryHandler : IRequestHandler<GetMentorsBySkillsQuery, List<MentorMatchDto>>
{
    private readonly IUserService _userService;

    public GetMentorsBySkillsQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<List<MentorMatchDto>> Handle(GetMentorsBySkillsQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetMentorsBySkillsWithScoreAsync(request.Skills);
    }
}