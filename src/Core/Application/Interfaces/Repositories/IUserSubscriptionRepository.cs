using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserSubscriptionRepository
{
    Task CreateAsync(UserSubscription subscription);
    Task<SubscriptionDto?> GetActiveSubscriptionByUserIdAsync(string userId);
    Task<List<SubscriptionDto>> GetAllByUserIdAsync(string userId);
    Task<SubscriptionDto?> GetByUserIdAsync(string userId);
    Task<List<SubscriptionDto>> GetAllAsync();
    Task<bool> UpdateAsync(string id, UserSubscription updatedEntity);
    Task<bool> DeleteAsync(string id);
}