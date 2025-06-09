using User.Domain.Entities;

namespace User.Domain.Repositories;

public interface IActivityLogRepository
{
    Task LogAsync(ActivityLog log);
    Task<List<ActivityLog>> GetLogsByUserIdAsync(string userId);
}