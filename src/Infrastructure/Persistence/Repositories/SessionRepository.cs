using Application.Interfaces;
using MongoDB.Driver;
using User.Domain.Entities;
using UserEntity=User.Domain.Entities.User;
namespace Infrastructure.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly IMongoCollection<Session> _sessionsCollection;
    private readonly IMongoCollection<UserEntity> _users;

    public SessionRepository(IMongoDatabase database)
    {
        _sessionsCollection = database.GetCollection<Session>("Sessions");
        _users = database.GetCollection<UserEntity>("Users");

    }

    public async Task<List<Session>> GetAllAsync() => await _sessionsCollection.Find(_ => true).ToListAsync();
    public async Task<Session?> GetByIdAsync(string id) => await _sessionsCollection.Find(s => s.Id == id).FirstOrDefaultAsync();
    public async Task CreateAsync(Session session) => await _sessionsCollection.InsertOneAsync(session);
    public async Task UpdateAsync(Session session) => await _sessionsCollection.ReplaceOneAsync(s => s.Id == session.Id, session);
    public async Task DeleteAsync(string id) => await _sessionsCollection.DeleteOneAsync(s => s.Id == id);
    public async Task<List<Session>> GetSessionsByUserIdAsync(string userId) =>
        await _sessionsCollection.Find(s => s.MentorId == userId || s.MenteeId == userId).ToListAsync();
    
    public async Task<List<MentorAvailability>> GetMentorAvailabilityAsync(string mentorId)
    {
        var user = await _users.Find(u => u.Id == mentorId).FirstOrDefaultAsync();
        return user?.Availabilities ?? new List<MentorAvailability>();
    }
    public async Task UpdateMentorAvailabilityAsync(string mentorId, List<MentorAvailability> updatedAvailability)
    {
        var update = Builders<UserEntity>.Update.Set(u => u.Availabilities, updatedAvailability);
        await _users.UpdateOneAsync(u => u.Id == mentorId, update);
    }

}
