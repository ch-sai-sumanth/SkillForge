namespace User.Domain.Entities;

public class Session
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string MentorId { get; set; } = string.Empty;
    public string MenteeId { get; set; } = string.Empty;
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string Topic { get; set; } = string.Empty;
    public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled

    public string? MeetingLink { get; set; } // Optional Zoom/Meet/etc
    public string? Notes { get; set; } // Optional summary
}