using Application.Interfaces;
using MediatR;

namespace Application.Commands.LogoutUser;

public class LogoutUserCommandHandler:IRequestHandler<LogoutUserCommand, bool>
{
    private readonly IAuthService _authService;

    public LogoutUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<bool> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(request.UserId);
        return true;
    }
}