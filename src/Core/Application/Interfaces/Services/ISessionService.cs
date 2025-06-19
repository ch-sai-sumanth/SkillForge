using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface ISessionService
{
    Task<List<SessionDto>> GetUserSessionsAsync(string userId);
    Task<SessionDto> BookSessionAsync(BookSessionDto dto);
    Task<bool> CancelSessionAsync(string sessionId);

    Task<SessionDto?> GetSessionByIdAsync(string sessionId);
    Task UpdateSessionAsync(string id,UpdateSessionDto updateSessionDto);
    Task AcceptSessionAsync(string sessionId);
    Task DeclineSessionAsync(string sessionId);
    Task<List<SessionDto>> GetPendingSessionsForMentorAsync(string mentorId);
    Task<bool> CompleteSessionAsync(string sessionId);
    Task<List<SessionHistoryDto>> GetSessionHistoryAsync(string sessionId);
}