using Application.Interfaces;
using MediatR;

namespace Application.Commands.User;

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand>
{
    private readonly IUserService _userService;

    public UpdateUserRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async System.Threading.Tasks.Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(request.Id);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.Role = request.NewRole;
        await _userService.UpdateAsync(user);
    }
}