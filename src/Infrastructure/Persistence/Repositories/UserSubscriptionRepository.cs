using Application.Interfaces.Repositories;
using MongoDB.Driver;
using User.Domain.Entities;

namespace Infrastructure.Repositories;

public class UserSubscriptionRepository : IUserSubscriptionRepository
    
{
    private readonly IMongoCollection<UserSubscription> _collection;

    public UserSubscriptionRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<UserSubscription>("UserSubscriptions");
    }

    // CREATE
    public async Task CreateAsync(UserSubscription subscription) =>
        await _collection.InsertOneAsync(subscription);

    // READ (Single Active)
    public async Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(string userId) =>
        await _collection.Find(s => s.UserId == userId && s.ExpiresAt >= DateTime.UtcNow)
            .FirstOrDefaultAsync();

    // READ (All by User)
    public async Task<List<UserSubscription>> GetAllByUserIdAsync(string userId) =>
        await _collection.Find(s => s.UserId == userId).ToListAsync();

    // READ (Latest by User - for showing most recent subscription)
    public async Task<UserSubscription?> GetByUserIdAsync(string userId) =>
        await _collection.Find(s => s.UserId == userId)
            .SortByDescending(s => s.SubscribedAt)
            .FirstOrDefaultAsync();

    // UPDATE
    public async Task<bool> UpdateAsync(string id, UserSubscription updatedSubscription)
    {
        var result = await _collection.ReplaceOneAsync(
            s => s.Id == id,
            updatedSubscription);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(s => s.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public Task<List<UserSubscription>> GetAllAsync()
    {
        return _collection.Find(_ => true).ToListAsync();
    }
}