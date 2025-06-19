using Application.DTOs;
using MediatR;

namespace Application.Queries.GetSession;

public record GetPendingSessionsForMentorQuery(string MentorId) : IRequest<List<SessionDto>>;
