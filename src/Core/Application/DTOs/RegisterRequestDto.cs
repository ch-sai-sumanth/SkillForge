using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class RegisterRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
    
    public string Role { get; set; }= string.Empty; 
    public List<string> Skills { get; set; } = new();
}
