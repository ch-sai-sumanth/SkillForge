namespace User.Domain.Entities;

public class MentorRate
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string MentorId { get; set; } = null!;
    public decimal HourlyRate { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public object Currency { get; set; }
}