using Application.DTOs;
using MediatR;

namespace Application.Commands.User;

public class CreateUserCommand : IRequest
{
    public UserDto UserDto { get; set; }
}