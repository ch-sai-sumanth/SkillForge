using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using User.Domain.Entities;

namespace Application.Services;

public class UserSubscriptionService : IUserSubscriptionService
{
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly IMapper _mapper;

    public UserSubscriptionService(IUserSubscriptionRepository subscriptionRepository, IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
    }

    public async Task SubscribeUserAsync(SubscribeUserDto dto)
    {
        var subscription = _mapper.Map<UserSubscription>(dto);

        subscription.Id = Guid.NewGuid().ToString();
        subscription.SubscribedAt = DateTime.UtcNow;
        subscription.ExpiresAt = DateTime.UtcNow.AddDays(dto.DurationInDays);

        await _subscriptionRepository.CreateAsync(subscription);
    }

    public async Task<UserSubscriptionDto?> GetSubscriptionDtoByUserIdAsync(string userId)
    {
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
        return subscription == null ? null : _mapper.Map<UserSubscriptionDto>(subscription);
    }

    public async Task<UserSubscription?> GetSubscriptionByUserAsync(string userId)
    {
        return await _subscriptionRepository.GetByUserIdAsync(userId);
    }

    public async Task<bool> IsUserSubscribedAsync(string userId)
    {
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
        return subscription != null && DateTime.UtcNow <= subscription.ExpiresAt;
    }

    public async Task<List<UserSubscriptionDto>> GetAllSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetAllAsync();
        return _mapper.Map<List<UserSubscriptionDto>>(subscriptions);
    }

    public async Task<bool> CancelSubscriptionAsync(string userId)
    {
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
        if (subscription == null || DateTime.UtcNow > subscription.ExpiresAt)
            return false;

        subscription.ExpiresAt = DateTime.UtcNow; // Cancel immediately
        subscription.IsActive = false;
        var success = await _subscriptionRepository.UpdateAsync(subscription.Id, subscription);
        return success;
    }
}