using Application.Interfaces;
using MediatR;

namespace Application.Commands.Task.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand,bool>
{
    private readonly ITaskService _taskService;

    public UpdateTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskService.UpdateTaskAsync(request.TaskId, request.Dto);
        return true;
    }
}
