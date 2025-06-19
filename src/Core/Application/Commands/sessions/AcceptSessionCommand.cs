using MediatR;

namespace Application.Commands.sessions;

public record AcceptSessionCommand(string SessionId) : IRequest<bool>;
