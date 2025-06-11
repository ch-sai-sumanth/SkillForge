namespace User.Domain.Entities;

public class LearningGoal
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime TargetDate { get; set; }

    public string MenteeId { get; set; } = null!;

    public string Status { get; set; } = "InProgress"; // e.g., InProgress, Achieved, Dropped
    public int ProgressPercentage { get; set; } = 0; // e.g., 0 to 100
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}