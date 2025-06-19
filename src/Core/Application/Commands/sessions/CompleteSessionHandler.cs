using Application.Interfaces;
using MediatR;

namespace Application.Commands.sessions;

public class CompleteSessionHandler : IRequestHandler<CompleteSessionCommand,bool>
{
    private readonly ISessionService _service;
    public CompleteSessionHandler(ISessionService service) => _service = service;
    public Task<bool> Handle(CompleteSessionCommand request, CancellationToken ct)
    {
        return _service.CompleteSessionAsync(request.SessionId);
    }
}