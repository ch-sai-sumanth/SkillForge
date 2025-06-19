using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ISubscriptionPlanRepository
{
    Task CreateAsync(SubscriptionPlan plan);
    Task<List<SubscriptionPlan>> GetAllPlansAsync();
    Task<SubscriptionPlan?> GetByIdAsync(string id);
    Task<bool> UpdateAsync(string id, SubscriptionPlan updatedPlan);
    Task<bool> DeleteAsync(string id);
}