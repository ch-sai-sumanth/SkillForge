using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetSession;

public class GetSessionByIdHandler : IRequestHandler<GetSessionByIdQuery, SessionDto?>
{
    private readonly ISessionService _service;
    public GetSessionByIdHandler(ISessionService service) => _service = service;
    public Task<SessionDto?> Handle(GetSessionByIdQuery request, CancellationToken ct) => _service.GetSessionByIdAsync(request.SessionId);
}