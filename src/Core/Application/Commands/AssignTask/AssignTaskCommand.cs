using Application.DTOs;
using MediatR;

namespace Application.Commands.Task.AssignTask;

public class AssignTaskCommand : IRequest
{
    public AssignTaskDto Dto { get; set; }
}