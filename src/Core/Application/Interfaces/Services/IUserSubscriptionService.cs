using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface IUserSubscriptionService
{
    Task SubscribeUserAsync(SubscribeUserDto dto);
    Task<UserSubscriptionDto?> GetSubscriptionDtoByUserIdAsync(string userId);
    Task<UserSubscription?> GetSubscriptionByUserAsync(string userId);
    Task<bool> IsUserSubscribedAsync(string userId);
    Task<List<UserSubscriptionDto>> GetAllSubscriptionsAsync();
    Task<bool> CancelSubscriptionAsync(string userId);
}