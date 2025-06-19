using MediatR;

namespace Application.Commons;

public class RegisterUserCommand:IRequest<string>
{
    public string name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = new();
}