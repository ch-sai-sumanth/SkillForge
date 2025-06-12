using Application.DTOs;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface IMentorRateService
{
    Task SetMentorRateAsync(MentorRateDto dto);
    Task<MentorRateDto?> GetMentorRateAsync(string mentorId);
    Task<MentorRate?> GetRateForMentorAsync(string mentorId);
    Task<bool> DeleteMentorRateAsync(string mentorId);

}