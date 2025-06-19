using Application.DTOs;
using MediatR;

namespace Application.Queries.Task.GetTask;

public class GetTaskByIdQuery : IRequest<TaskDto?>
{
    public string TaskId { get; set; }

    public GetTaskByIdQuery(string taskId) => TaskId = taskId;
}
