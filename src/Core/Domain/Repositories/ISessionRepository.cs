using User.Domain.Entities;

namespace Application.Interfaces;

public interface ISessionRepository
{
    
    Task<List<Session>> GetAllAsync();
    Task<Session?> GetByIdAsync(string id);
    Task CreateAsync(Session session);
    Task UpdateAsync(Session session);
    Task DeleteAsync(string id);
    Task<List<Session>> GetSessionsByUserIdAsync(string userId);
    Task<List<MentorAvailability>> GetMentorAvailabilityAsync(string mentorId);
    Task UpdateMentorAvailabilityAsync(string mentorId, List<MentorAvailability> updatedAvailability);


}