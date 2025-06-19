using Application.Interfaces;
using MediatR;
namespace Application.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async System.Threading.Tasks.Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        request.UserDto.Id = string.Empty;
        await _userService.CreateAsync(request.UserDto);
    }
}