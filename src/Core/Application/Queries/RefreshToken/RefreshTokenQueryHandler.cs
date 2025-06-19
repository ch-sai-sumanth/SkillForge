using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.RefreshToken;

public class RefreshTokenQueryHandler:IRequestHandler<RefreshTokenQuery, AuthResponseDto>
{
    private readonly IAuthService _authService;

    public RefreshTokenQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<AuthResponseDto> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        return await _authService.RefreshTokenAsync(request.RefreshToken);
    }
}