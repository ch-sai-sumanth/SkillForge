using MediatR;

namespace Application.Commands.sessions;

public record CompleteSessionCommand(string SessionId) : IRequest<bool>;
