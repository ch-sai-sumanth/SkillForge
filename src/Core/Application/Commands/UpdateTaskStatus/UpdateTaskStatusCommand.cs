using MediatR;

namespace Application.Commands.Task.UpdateTaskStatus;

public class UpdateTaskStatusCommand : IRequest<bool>
{
    public string TaskId { get; set; }
    public string Status { get; set; }
}