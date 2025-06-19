using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.AvailableMentors;

public class GetAvailableMentorsQueryHandler : IRequestHandler<GetAvailableMentorsQuery, List<MentorMatchDto>>
{
    private readonly IUserService _userService;

    public GetAvailableMentorsQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<List<MentorMatchDto>> Handle(GetAvailableMentorsQuery request, CancellationToken cancellationToken)
    {
        return await _userService.SearchMentorsBySkillAndAvailabilityAsync(request.Skill, request.DateTime);
    }
}