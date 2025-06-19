using Application.DTOs;
using MediatR;

namespace Application.Queries.RefreshToken;

public class RefreshTokenQuery:IRequest<AuthResponseDto>
{
    public string RefreshToken { get; set; }
}