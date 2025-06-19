using MediatR;

namespace Application.Commands.User;

public class UpdateUserRoleCommand : IRequest
{
    public string Id { get; set; }
    public string NewRole { get; set; }
}