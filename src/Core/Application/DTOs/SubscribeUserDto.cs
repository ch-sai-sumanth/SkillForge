namespace Application.DTOs;

public class SubscribeUserDto
{
    public string UserId { get; set; } = null!;
    public string PlanId { get; set; } = null!;
    public int DurationInDays { get; set; }
}