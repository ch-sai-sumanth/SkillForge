using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.Task.GetTask;

public class GetTasksForMenteeQueryHandler : IRequestHandler<GetTasksForMenteeQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskService _taskService;

    public GetTasksForMenteeQueryHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksForMenteeQuery request, CancellationToken cancellationToken)
    {
        return await _taskService.GetTasksByMenteeIdAsync(request.MenteeId);
    }
}
