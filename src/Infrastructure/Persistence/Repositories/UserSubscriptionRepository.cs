using MongoDB.Driver;

namespace Infrastructure.Repositories;

using Application.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using MongoDB.Driver;
using User.Domain.Entities;

public class UserSubscriptionRepository : IUserSubscriptionRepository
{
    private readonly IMongoCollection<UserSubscription> _collection;
    private readonly IMapper _mapper;

    public UserSubscriptionRepository(IMongoDatabase database, IMapper mapper)
    {
        _collection = database.GetCollection<UserSubscription>("UserSubscriptions");
        _mapper = mapper;
    }

    // CREATE
    public async Task CreateAsync(UserSubscription subscription) =>
        await _collection.InsertOneAsync(subscription);

    // READ (Single Active)
    public async Task<SubscriptionDto?> GetActiveSubscriptionByUserIdAsync(string userId)
    {
        var sub = await _collection
            .Find(s => s.MenteeId == userId && s.EndDate >= DateTime.UtcNow)
            .FirstOrDefaultAsync();

        return sub is null ? null : _mapper.Map<SubscriptionDto>(sub);
    }

    // READ (All by Mentee)
    public async Task<List<SubscriptionDto>> GetAllByUserIdAsync(string userId)
    {
        var subs = await _collection.Find(s => s.MenteeId == userId).ToListAsync();
        return _mapper.Map<List<SubscriptionDto>>(subs);
    }

    // READ (Latest by Mentee)
    public async Task<SubscriptionDto?> GetByUserIdAsync(string userId)
    {
        var sub = await _collection.Find(s => s.MenteeId == userId)
            .SortByDescending(s => s.StartDate)
            .FirstOrDefaultAsync();

        return sub is null ? null : _mapper.Map<SubscriptionDto>(sub);
    }

    // UPDATE
    public async Task<bool> UpdateAsync(string id, UserSubscription updatedEntity)
    {
        var result = await _collection.ReplaceOneAsync(s => s.Id == id, updatedEntity);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(s => s.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    // READ (All)
    public async Task<List<SubscriptionDto>> GetAllAsync()
    {
        var subs = await _collection.Find(_ => true).ToListAsync();
        return _mapper.Map<List<SubscriptionDto>>(subs);
    }
}
