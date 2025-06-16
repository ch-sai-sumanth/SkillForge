using Domain;

namespace User.Domain.Entities;

public class Session
{
    public string Id { get; set; }
    public string MentorId { get; set; }
    public string MenteeId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Topic { get; set; }
    public SessionStatus Status { get; set; } // Pending, Scheduled, Declined, Cancelled
    public string? MeetingLink { get; set; }
    public string? Notes { get; set; }
}
