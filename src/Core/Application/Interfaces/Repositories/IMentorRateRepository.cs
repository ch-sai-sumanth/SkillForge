using User.Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IMentorRateRepository
{
    Task<MentorRate?> GetByMentorIdAsync(string mentorId);
    Task<MentorRate?> GetRateByMentorIdAsync(string mentorId);
    Task SetOrUpdateRateAsync(MentorRate rate);
    Task UpsertRateAsync(MentorRate rate);
    Task<bool> DeleteRateAsync(string mentorId);
}