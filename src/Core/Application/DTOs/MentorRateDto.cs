namespace Application.DTOs;

public class MentorRateDto
{
    public string MentorId { get; set; } = null!;
    public decimal HourlyRate { get; set; }
    public string Currency { get; set; } = "INR";
}