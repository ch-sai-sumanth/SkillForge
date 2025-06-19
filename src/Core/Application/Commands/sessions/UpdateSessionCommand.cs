using Application.DTOs;
using MediatR;

namespace Application.Commands.sessions;

public record UpdateSessionCommand(string SessionId, UpdateSessionDto Dto) : IRequest;
