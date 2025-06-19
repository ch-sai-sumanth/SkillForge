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
        subscription.StartDate = DateTime.UtcNow;
        subscription.EndDate = subscription.StartDate.AddDays(dto.DurationInDays);
        subscription.IsConfirmed = false; // Mentor needs to confirm

        await _subscriptionRepository.CreateAsync(subscription);
    }

    public async Task<SubscriptionDto?> GetSubscriptionDtoByUserIdAsync(string userId)
    {
        return await _subscriptionRepository.GetByUserIdAsync(userId);
    }

    public async Task<SubscriptionDto?> GetSubscriptionByUserAsync(string userId)
    {
        return await _subscriptionRepository.GetByUserIdAsync(userId);
    }

    public async Task<bool> IsUserSubscribedAsync(string userId)
    {
        var subscription = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(userId);
        return subscription != null;
    }

    public async Task<List<SubscriptionDto>> GetAllSubscriptionsAsync()
    {
        return await _subscriptionRepository.GetAllAsync();
    }

    public async Task<bool> CancelSubscriptionAsync(string userId)
    {
        var subscriptionDto = await _subscriptionRepository.GetByUserIdAsync(userId);
        if (subscriptionDto == null || subscriptionDto.EndDate < DateTime.UtcNow)
            return false;

        var updatedEntity = _mapper.Map<UserSubscription>(subscriptionDto);
        updatedEntity.EndDate = DateTime.UtcNow;

        return await _subscriptionRepository.UpdateAsync(updatedEntity.Id, updatedEntity);
    }
}