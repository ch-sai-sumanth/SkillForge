using Application.DTOs;
using MediatR;

namespace Application.Commands.User;

public class UpdateUserCommand : IRequest
{
    public string Id { get; set; }
    public UserDto UserDto { get; set; }
}