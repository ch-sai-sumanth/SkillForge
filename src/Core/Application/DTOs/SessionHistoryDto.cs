namespace Application.DTOs;

public class SessionHistoryDto
{
    public string Id { get; set; } = string.Empty;

    public string SessionId { get; set; } = string.Empty;

    public string OldStatus { get; set; } = string.Empty;  // Enum as string

    public string NewStatus { get; set; } = string.Empty;  // Enum as string

    public DateTime ChangedAt { get; set; }

    public string ChangedBy { get; set; } = string.Empty;  // "Mentor" or "Mentee"

    public string ChangeReason { get; set; } = "Rescheduled";
}