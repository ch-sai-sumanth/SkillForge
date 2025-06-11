namespace Application.DTOs;

public class AssignTaskDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public string MenteeId { get; set; } = null!;
    public string MentorId { get; set; } = null!;
}