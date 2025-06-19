using Application.Interfaces;
using MediatR;

namespace Application.Commands.MentorAvailability;

public class SetMentorAvailabilityCommandHandler : IRequestHandler<SetMentorAvailabilityCommand, bool>
{
    private readonly IUserService _userService;

    public SetMentorAvailabilityCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(SetMentorAvailabilityCommand request, CancellationToken cancellationToken)
    {
        await _userService.SetMentorAvailabilityAsync(request.MentorId, request.Availabilities);
        return true;
    }
}