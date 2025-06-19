using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetProfile;

public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, UserDto>
{
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;

    public GetMyProfileQueryHandler(IUserService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    public async Task<UserDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        return await _userService.GetByIdAsync(userId);
    }
}