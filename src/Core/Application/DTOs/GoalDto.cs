namespace Application.DTOs;

public class GoalDto
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime TargetDate { get; set; }
    public string MenteeId { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int ProgressPercentage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? AbandonedAt { get; set; }
    public string? AbandonReason { get; set; }
    
    // Computed properties for UI
    public bool IsOverdue { get; set; }
    public bool IsDueToday { get; set; }
    public bool IsDueSoon { get; set; }
    public int DaysRemaining { get; set; }
    public bool CanBeModified { get; set; }
}