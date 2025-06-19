using Application.Interfaces;
using MediatR;

namespace Application.Commands.sessions;

public class UpdateSessionHandler : IRequestHandler<UpdateSessionCommand>
{
    private readonly ISessionService _service;
    public UpdateSessionHandler(ISessionService service) => _service = service;
    public System.Threading.Tasks.Task Handle(UpdateSessionCommand request, CancellationToken ct)
    {
         return _service.UpdateSessionAsync(request.SessionId, request.Dto);
           
    }
}