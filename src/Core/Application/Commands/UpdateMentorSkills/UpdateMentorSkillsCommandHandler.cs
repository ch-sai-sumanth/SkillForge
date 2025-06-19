using Application.Interfaces;
using MediatR;

namespace Application.Commands.UpdateMentorSkills;

public class UpdateMentorSkillsCommandHandler : IRequestHandler<UpdateMentorSkillsCommand, bool>
{
    private readonly IUserService _userService;

    public UpdateMentorSkillsCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(UpdateMentorSkillsCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdateMentorSkillsAsync(request.MentorId, request.Skills);
    }
}