namespace Application.DTOs;

public class CreateGoalDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime TargetDate { get; set; }
    public string MenteeId { get; set; } = null!;
}