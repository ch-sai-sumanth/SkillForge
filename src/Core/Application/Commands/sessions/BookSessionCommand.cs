using Application.DTOs;
using MediatR;

namespace Application.Commands.sessions;

public record BookSessionCommand(BookSessionDto Dto) : IRequest<SessionDto>;
