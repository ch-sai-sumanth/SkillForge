namespace User.Domain.Entities;

public class UserSubscription
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string PlanId { get; set; } = null!;
    public DateTime SubscribedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
}