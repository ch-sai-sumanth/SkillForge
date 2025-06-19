namespace Application.DTOs;

public class TaskDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DueDate { get; set; }

    public string MentorId { get; set; } = null!;
    public string MenteeId { get; set; } = null!;

    public string Status { get; set; } = "Pending"; // e.g., Pending, Completed, Overdue
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? CompletedDate { get; set; }
}