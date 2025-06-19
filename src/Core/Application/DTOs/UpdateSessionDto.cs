namespace Application.DTOs;

public class UpdateSessionDto
{
    public string Topic { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? MeetingLink { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty; // Stored as enum internally, mapped to string in DTO
    public string? UpdatedBy { get; set; } // Optional tracking
}