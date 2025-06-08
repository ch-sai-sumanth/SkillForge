namespace Application.DTOs;

public class UpdateUserProfileDto
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = new();
}