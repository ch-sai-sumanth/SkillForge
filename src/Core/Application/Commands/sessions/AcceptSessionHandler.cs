using Application.Interfaces;
using MediatR;

namespace Application.Commands.sessions;

public class AcceptSessionHandler : IRequestHandler<AcceptSessionCommand,bool>
{
    private readonly ISessionService _service;
    public AcceptSessionHandler(ISessionService service) => _service = service;
    
    public async Task<bool> Handle(AcceptSessionCommand request, CancellationToken ct)
    {
        await _service.AcceptSessionAsync(request.SessionId);
        return true;
    }
}