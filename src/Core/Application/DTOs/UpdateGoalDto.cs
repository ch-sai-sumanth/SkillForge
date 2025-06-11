namespace Application.DTOs;

public class UpdateGoalDto
{
    public string GoalId { get; set; } = default!;
    public int? ProgressPercentage { get; set; } 
    public string? Status { get; set; }     
}