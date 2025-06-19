using Application.DTOs;
using MediatR;

namespace Application.Commons.LoginUser;

public class LoginUserQuery : IRequest<AuthResponseDto>
{
    public string Username { get; set; }    
    public string Password { get; set; }
}