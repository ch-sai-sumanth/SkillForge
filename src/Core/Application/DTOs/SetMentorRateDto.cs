namespace Application.DTOs;

public class SetMentorRateDto
{
    public string MentorId { get; set; } = null!;
    public decimal HourlyRate { get; set; }
    public object Currency { get; set; }
}