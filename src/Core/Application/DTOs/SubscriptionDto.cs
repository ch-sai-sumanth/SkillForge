namespace Application.DTOs;

public class SubscriptionDto
{
    public string Id { get; set; }
    public string MenteeId { get; set; }
    public string MentorId { get; set; }
    public string PlanId { get; set; }

    public string Title { get; set; }          // From plan
    public decimal Price { get; set; }         // From plan
    public int DurationInDays { get; set; }    // From plan

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsConfirmed { get; set; }      // Confirmed by mentor?
}