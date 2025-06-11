namespace User.Domain.Entities;

public class User
{
    public string Id { get; set; } = string.Empty; 
    public string Name { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Mentee";
    public List<string> Skills { get; set; } = new();
    
    public List<MentorAvailability> Availabilities { get; set; } = new();
    
    public string? ProfileImagePath { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

}