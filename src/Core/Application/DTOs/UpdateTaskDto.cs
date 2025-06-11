namespace Application.DTOs;

public class UpdateTaskDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; }
    public DateTime DueDate { get; set; }
}