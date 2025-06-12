using User.Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserSubscriptionRepository
{
    Task CreateAsync(UserSubscription subscription);
    Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(string userId);
    Task<List<UserSubscription>> GetAllByUserIdAsync(string userId);
    Task<UserSubscription?> GetByUserIdAsync(string userId);
    Task<bool> UpdateAsync(string id, UserSubscription updatedSubscription);
    Task<bool> DeleteAsync(string id);
    Task<List<UserSubscription>> GetAllAsync();
}