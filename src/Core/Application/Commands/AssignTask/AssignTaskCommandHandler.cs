using Application.Interfaces;
using MediatR;

namespace Application.Commands.Task.AssignTask;

public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand>
{
    private readonly ITaskService _taskService;

    public AssignTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async System.Threading.Tasks.Task Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {
        await _taskService.AssignTaskAsync(request.Dto);
    }
}
