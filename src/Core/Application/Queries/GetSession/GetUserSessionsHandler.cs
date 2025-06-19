using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetSession;

public class GetUserSessionsHandler : IRequestHandler<GetUserSessionsQuery, List<SessionDto>>
{
    private readonly ISessionService _service;
    public GetUserSessionsHandler(ISessionService service) => _service = service;
    public Task<List<SessionDto>> Handle(GetUserSessionsQuery request, CancellationToken ct) => _service.GetUserSessionsAsync(request.UserId);
}