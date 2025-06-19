namespace User.Domain.Entities;

public class UserSubscription
{
    public string Id { get; set; } = string.Empty;

    public string MenteeId { get; set; } = string.Empty;

    public string MentorId { get; set; } = string.Empty;

    public string PlanId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int DurationInDays { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsConfirmed { get; set; } = false;
}