namespace User.Domain.Entities;

public class TaskAssignment
{
    public string Id { get; set; }
    public string SessionId { get; set; }
    public string MentorId { get; set; }
    public string MenteeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "Assigned"; // or InProgress, Completed
    public DateTime AssignedAt { get; set; }
}