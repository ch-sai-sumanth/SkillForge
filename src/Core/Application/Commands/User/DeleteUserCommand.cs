using MediatR;

namespace Application.Commands.User;

public class DeleteUserCommand : IRequest
{
    public string Id { get; set; }
}