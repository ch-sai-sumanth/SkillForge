using Application.DTOs;
using MediatR;

namespace Application.Queries.GetSession;

public record GetUserSessionsQuery(string UserId) : IRequest<List<SessionDto>>;
