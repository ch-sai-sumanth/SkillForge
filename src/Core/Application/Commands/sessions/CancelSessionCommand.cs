using MediatR;

namespace Application.Commands.sessions;

public record CancelSessionCommand(string SessionId) : IRequest<bool>;
