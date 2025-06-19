using Application.Interfaces;
using MediatR;

namespace Application.Commands.sessions;

public class CancelSessionHandler : IRequestHandler<CancelSessionCommand,bool>
{
    private readonly ISessionService _service;
    public CancelSessionHandler(ISessionService service) => _service = service;
    
    public async Task<bool> Handle(CancelSessionCommand request, CancellationToken ct)
    {
        return await _service.CancelSessionAsync(request.SessionId);

    }
}