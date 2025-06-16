using Domain;

namespace User.Domain.Entities;

public class SessionHistory
{
    public string Id { get; set; }
    public string SessionId { get; set; }
    public SessionStatus OldStatus { get; set; }
    public SessionStatus NewStatus { get; set; }
    public DateTime ChangedAt { get; set; }
    public string ChangedBy { get; set; } // Mentor or Mentee
    public string ChangeReason { get; set; } = "Rescheduled";
}
