using Application.DTOs;
using MediatR;

namespace Application.Queries.GetSession;

public record GetSessionByIdQuery(string SessionId) : IRequest<SessionDto?>;
