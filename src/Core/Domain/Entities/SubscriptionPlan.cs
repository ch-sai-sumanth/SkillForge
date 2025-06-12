namespace User.Domain.Entities;

public class SubscriptionPlan
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int DurationInDays { get; set; } // e.g., 30 for monthly
    public int MaxSessions { get; set; } // Number of sessions allowed in the plan
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}