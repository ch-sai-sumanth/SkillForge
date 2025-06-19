using MediatR;

namespace Application.Commands.LogoutUser;

public class LogoutUserCommand : IRequest<bool>
{
    public string UserId { get; set; }
    
}