using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Commands.UpdateProfile;

public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand, UserDto>
{
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;

    public UpdateMyProfileCommandHandler(IUserService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    public async Task<UserDto> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        return await _userService.UpdateProfileAsync(userId, request.Dto);
    }
}