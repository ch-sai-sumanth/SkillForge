using MediatR;

namespace Application.Commands.Task.DeleteTask;

public class DeleteTaskCommand : IRequest<bool>
{
    public string TaskId { get; set; }
}