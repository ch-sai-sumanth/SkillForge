using Application.DTOs;
using MediatR;

namespace Application.Queries.Task.GetTask;

public class GetTasksForMenteeQuery : IRequest<IEnumerable<TaskDto>>
{
    public string MenteeId { get; set; }

    public GetTasksForMenteeQuery(string menteeId) => MenteeId = menteeId;
}
