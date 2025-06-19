using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Commons;

public class RegisterUserCommandHandler:IRequestHandler<RegisterUserCommand, string>
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IUserService userService,IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }
    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userService.GetByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username is already taken.");
        }
        var existingEmail = await _userService.GetByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            throw new InvalidOperationException("An account with this email already exists.");
        }
        await _authService.CreateUserAsync(new RegisterRequestDto
        {
            Name=request.name,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            Role = request.Role,
            Skills = request.Skills
        });
        return "User registered successfully";
    }
}