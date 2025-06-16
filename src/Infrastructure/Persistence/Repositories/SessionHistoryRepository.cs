using Application.Interfaces;
using MongoDB.Driver;
using User.Domain.Entities;

namespace Infrastructure.Repositories;

public class SessionHistoryRepository : ISessionHistoryRepository
{
    private readonly IMongoCollection<SessionHistory> _collection;

    public SessionHistoryRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<SessionHistory>("SessionHistories");
    }

    public async Task AddAsync(SessionHistory history)
    {
        await _collection.InsertOneAsync(history);
    }

    public async Task<List<SessionHistory>> GetBySessionIdAsync(string sessionId)
    {
        var filter = Builders<SessionHistory>.Filter.Eq(h => h.SessionId, sessionId);
        return await _collection.Find(filter).SortByDescending(h => h.ChangedAt).ToListAsync();
    }
}