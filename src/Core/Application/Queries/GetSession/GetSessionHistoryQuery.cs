using Application.DTOs;
using MediatR;

namespace Application.Queries.GetSession;

public record GetSessionHistoryQuery(string SessionId) : IRequest<List<SessionHistoryDto>>;
