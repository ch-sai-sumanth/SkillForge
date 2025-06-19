namespace Application.DTOs;

public class SessionDto
{
    public string Id { get; set; } = string.Empty;

    public string MentorId { get; set; } = string.Empty;
    public string MenteeId { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string Topic { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty; // Stored as enum internally, mapped to string in DTO

    public string? MeetingLink { get; set; }
    public string? Notes { get; set; }
}