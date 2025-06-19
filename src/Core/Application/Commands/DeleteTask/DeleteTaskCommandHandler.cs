using Application.Interfaces;
using MediatR;

namespace Application.Commands.Task.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand,bool>
{
    private readonly ITaskService _taskService;

    public DeleteTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskService.DeleteTaskAsync(request.TaskId);
        return true;
    }
}
