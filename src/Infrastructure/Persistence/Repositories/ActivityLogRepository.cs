using MongoDB.Driver;
using User.Domain.Entities;
using User.Domain.Repositories;

namespace Infrastructure.Repositories;

public class ActivityLogRepository : IActivityLogRepository
{
    private readonly IMongoCollection<ActivityLog> _logCollection;

    public ActivityLogRepository(IMongoDatabase database)
    {
        _logCollection = database.GetCollection<ActivityLog>("ActivityLogs");
    }
    
    public async Task LogAsync(ActivityLog log) =>
        await _logCollection.InsertOneAsync(log);

    public async Task<List<ActivityLog>> GetLogsByUserIdAsync(string userId) =>
        await _logCollection.Find(log => log.UserId == userId).ToListAsync();
}