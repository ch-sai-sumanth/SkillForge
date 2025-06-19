using Application.Interfaces;
using MediatR;

namespace Application.Commands.User;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async System.Threading.Tasks.Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Id != request.UserDto.Id)
            throw new ArgumentException("ID mismatch");

        await _userService.UpdateAsync(request.UserDto);
    }
}