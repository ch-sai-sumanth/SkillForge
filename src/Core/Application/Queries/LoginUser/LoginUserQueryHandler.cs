using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Commons.LoginUser;

public class LoginUserQueryHandler: IRequestHandler<LoginUserQuery, AuthResponseDto>
{
    private readonly IAuthService _authService;

    public LoginUserQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<AuthResponseDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request.Username, request.Password);
    }
}