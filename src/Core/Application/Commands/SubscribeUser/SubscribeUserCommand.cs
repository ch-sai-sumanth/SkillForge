using Application.DTOs;
using MediatR;

namespace Application.Commands.SubscribeUser;
public class SubscribeUserCommand : IRequest<bool>
{
    public SubscribeUserDto Dto { get; set; }
}