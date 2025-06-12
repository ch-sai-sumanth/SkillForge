using User.Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task CreateAsync(Payment payment);
    Task<Payment?> GetByIdAsync(string id);
    Task<List<Payment>> GetAllAsync();

    Task<bool> UpdateAsync(Payment payment);
    Task<bool> DeleteAsync(string id);
    Task<List<Payment>> GetPaymentsByMenteeAsync(string userId);
}