using Application.Interfaces;
using MediatR;

namespace Application.Commands.Task.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand,bool>
{
    private readonly ITaskService _taskService;

    public UpdateTaskStatusCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        await _taskService.UpdateTaskStatusAsync(request.TaskId, request.Status);
        return true;
    }
}
