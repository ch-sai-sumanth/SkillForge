using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetSession;

public class GetPendingSessionsForMentorHandler : IRequestHandler<GetPendingSessionsForMentorQuery, List<SessionDto>>
{
    private readonly ISessionService _service;
    public GetPendingSessionsForMentorHandler(ISessionService service) => _service = service;
    public Task<List<SessionDto>> Handle(GetPendingSessionsForMentorQuery request, CancellationToken ct) => _service.GetPendingSessionsForMentorAsync(request.MentorId);
}