using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.Task.GetTask;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly ITaskService _taskService;

    public GetTaskByIdQueryHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _taskService.GetTaskByIdAsync(request.TaskId);
    }
}
