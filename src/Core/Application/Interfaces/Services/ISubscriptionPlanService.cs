using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface ISubscriptionPlanService
{
    Task<string> CreatePlanAsync(CreateSubscriptionPlanDto dto);
    Task<List<SubscriptionPlanDto>> GetAllPlansAsync();
    Task<SubscriptionPlanDto?> GetPlanByIdAsync(string id);
    Task<bool> DeletePlanAsync(string id);
    Task<bool> UpdatePlanAsync(string id, UpdateSubscriptionPlanDto dto);
}