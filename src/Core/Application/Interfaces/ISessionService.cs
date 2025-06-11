using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface ISessionService
{
    Task<List<Session>> GetUserSessionsAsync(string userId);
    Task<Session> BookSessionAsync(BookSessionDto dto);
    Task CancelSessionAsync(string sessionId);

    Task<Session?> GetSessionByIdAsync(string sessionId);
}