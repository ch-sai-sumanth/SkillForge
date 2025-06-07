namespace Application.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Mentee";
    public List<string> Skills { get; set; } = new();
}