namespace Application.DTOs;

public class TaskDto
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending"; // Default: Pending, InProgress, Completed, etc.

    public string AssignedById { get; set; } = string.Empty; // Mentor ID

    public string AssignedToId { get; set; } = string.Empty; // Mentee ID

    public DateTime AssignedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }
}