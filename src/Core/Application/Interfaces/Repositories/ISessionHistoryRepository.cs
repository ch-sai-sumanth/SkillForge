using User.Domain.Entities;

namespace Application.Interfaces;

public interface ISessionHistoryRepository
{
    Task AddAsync(SessionHistory history);
    Task<List<SessionHistory>> GetBySessionIdAsync(string sessionId);

}