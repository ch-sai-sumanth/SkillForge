using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using User.Domain.Entities;

namespace Application.Services;

using AutoMapper;

public class SubscriptionPlanService : ISubscriptionPlanService
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IMapper _mapper;

    public SubscriptionPlanService(
        ISubscriptionPlanRepository planRepository,
        IMapper mapper)
    {
        _subscriptionPlanRepository = planRepository;
        _mapper = mapper;
    }

    public async Task CreatePlanAsync(CreateSubscriptionPlanDto dto)
    {
        var plan = _mapper.Map<SubscriptionPlan>(dto);
        plan.Id = Guid.NewGuid().ToString();
        plan.CreatedAt = DateTime.UtcNow;

        await _subscriptionPlanRepository.CreateAsync(plan);
    }

    public async Task CreateSubscriptionPlanAsync(SubscriptionPlanDto dto)
    {
        var plan = _mapper.Map<SubscriptionPlan>(dto);
        plan.Id = Guid.NewGuid().ToString();
        plan.CreatedAt = DateTime.UtcNow;

        await _subscriptionPlanRepository.CreateAsync(plan);
    }

    public async Task<List<SubscriptionPlanDto>> GetAllPlansAsync()
    {
        var plans = await _subscriptionPlanRepository.GetAllPlansAsync();
        return _mapper.Map<List<SubscriptionPlanDto>>(plans);
    }

    // 🚀 Suggested additional methods:

    public async Task<SubscriptionPlanDto?> GetPlanByIdAsync(string id)
    {
        var plan = await _subscriptionPlanRepository.GetByIdAsync(id);
        return plan == null ? null : _mapper.Map<SubscriptionPlanDto>(plan);
    }

    public async Task<bool> DeletePlanAsync(string id)
    {
        var plan = await _subscriptionPlanRepository.GetByIdAsync(id);
        if (plan == null)
            return false;

        await _subscriptionPlanRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> UpdatePlanAsync(string id, UpdateSubscriptionPlanDto dto)
    {
        var existingPlan = await _subscriptionPlanRepository.GetByIdAsync(id);
        if (existingPlan == null)
            return false;

        existingPlan.Title = dto.Title;
        existingPlan.Description = dto.Description;
        existingPlan.Price = dto.Price;
        existingPlan.DurationInDays = dto.DurationInDays;
        existingPlan.MaxSessions = dto.MaxSessions;

        await _subscriptionPlanRepository.UpdateAsync(id,existingPlan);
        return true;
    }
}
