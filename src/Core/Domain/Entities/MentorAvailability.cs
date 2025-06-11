namespace User.Domain.Entities;

public class MentorAvailability
{
    public string MentorId { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}