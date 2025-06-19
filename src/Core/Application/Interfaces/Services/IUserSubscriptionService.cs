using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface IUserSubscriptionService
{
    Task SubscribeUserAsync(SubscribeUserDto dto);
    Task<SubscriptionDto?> GetSubscriptionDtoByUserIdAsync(string userId);
    Task<SubscriptionDto?> GetSubscriptionByUserAsync(string userId);
    Task<bool> IsUserSubscribedAsync(string userId);
    Task<List<SubscriptionDto>> GetAllSubscriptionsAsync();
    Task<bool> CancelSubscriptionAsync(string userId);
}