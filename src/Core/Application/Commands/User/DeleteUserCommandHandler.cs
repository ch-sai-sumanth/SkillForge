using Application.Interfaces;
using MediatR;

namespace Application.Commands.User;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async System.Threading.Tasks.Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userService.DeleteAsync(request.Id);
    }
}