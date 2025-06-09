namespace Application.DTOs;

public class SkillMatchResponseDto
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<string> Skills { get; set; } = new();
    public double MatchScore { get; set; } // Higher score means better match
}