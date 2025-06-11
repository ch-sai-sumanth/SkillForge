namespace Application.DTOs;

public class BookSessionDto
{
    public string MentorId { get; set; } = string.Empty;
    public string MenteeId { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Topic { get; set; } = string.Empty;
}