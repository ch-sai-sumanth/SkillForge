using Application.DTOs;
using Application.Interfaces.Repositories;
using MongoDB.Driver;
using User.Domain.Entities;

namespace Infrastructure.Repositories;

public class SubscriptionPlanRepository : ISubscriptionPlanRepository
{
    private readonly IMongoCollection<SubscriptionPlan> _subscriptionPlanCollection;

    public SubscriptionPlanRepository(IMongoDatabase database)
    {
        _subscriptionPlanCollection = database.GetCollection<SubscriptionPlan>("SubscriptionPlans");
    }

    // CREATE
    public async Task CreateAsync(SubscriptionPlan plan) =>
        await _subscriptionPlanCollection.InsertOneAsync(plan);

    // READ (All Plans)
    public async Task<List<SubscriptionPlan>> GetAllAsync() =>
        await _subscriptionPlanCollection.Find(_ => true).ToListAsync();

    // READ (By ID)
    public async Task<SubscriptionPlan?> GetByIdAsync(string id) =>
        await _subscriptionPlanCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    

    // READ (All Plans - optional if you want to rename GetAllAsync)
    public async Task<List<SubscriptionPlan>> GetAllPlansAsync() =>
        await _subscriptionPlanCollection.Find(_ => true).ToListAsync();

    // UPDATE
    public async Task<bool> UpdateAsync(string id, SubscriptionPlan updatedPlan)
    {
        var result = await _subscriptionPlanCollection.ReplaceOneAsync(
            p => p.Id == id,
            updatedPlan);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _subscriptionPlanCollection.DeleteOneAsync(p => p.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}