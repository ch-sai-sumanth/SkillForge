namespace Application.DTOs;

public class AvailabilityDto
{
    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}