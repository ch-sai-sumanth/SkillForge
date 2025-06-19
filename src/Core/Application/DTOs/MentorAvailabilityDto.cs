namespace Application.DTOs;

public class MentorAvailabilityDto
{
    public string MentorId { get; set; } = string.Empty;
    public string MentorName { get; set; } = string.Empty;
    public List<DateTime> AvailableSlots { get; set; } = new();
}