using MediatR;

namespace Application.Commands.sessions;

public record DeclineSessionCommand(string SessionId) : IRequest;
