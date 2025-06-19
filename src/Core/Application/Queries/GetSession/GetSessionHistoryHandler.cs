using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetSession;

public class GetSessionHistoryHandler : IRequestHandler<GetSessionHistoryQuery, List<SessionHistoryDto>>
{
    private readonly ISessionService _service;
    public GetSessionHistoryHandler(ISessionService service) => _service = service;
    public Task<List<SessionHistoryDto>> Handle(GetSessionHistoryQuery request, CancellationToken ct) => _service.GetSessionHistoryAsync(request.SessionId);
}