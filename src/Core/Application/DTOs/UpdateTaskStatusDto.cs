namespace Application.DTOs;

public class UpdateTaskStatusDto
{
    public string TaskId { get; set; } = default!;
    public string Status { get; set; } = default!;
}