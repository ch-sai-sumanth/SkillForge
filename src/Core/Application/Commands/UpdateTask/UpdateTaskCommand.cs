using Application.DTOs;
using MediatR;

namespace Application.Commands.Task.UpdateTask;

public class UpdateTaskCommand : IRequest<bool>
{
    public string TaskId { get; set; }
    public UpdateTaskDto Dto { get; set; }
}
