using Application.Interfaces;
using MediatR;

namespace Application.Commands.sessions;

public class DeclineSessionHandler : IRequestHandler<DeclineSessionCommand>
{
    private readonly ISessionService _service;
    public DeclineSessionHandler(ISessionService service) => _service = service;
    public System.Threading.Tasks.Task Handle(DeclineSessionCommand request, CancellationToken ct)
    {
        return _service.DeclineSessionAsync(request.SessionId);
    }
}