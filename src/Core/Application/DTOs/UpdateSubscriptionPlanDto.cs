namespace Application.DTOs;

public class UpdateSubscriptionPlanDto
{
    public string MentorId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int DurationInDays { get; set; } // e.g., 30 for monthly, 90 for quarterly, etc.
    public int MaxSessions { get; set; } // Number of sessions allowed in the plan
}