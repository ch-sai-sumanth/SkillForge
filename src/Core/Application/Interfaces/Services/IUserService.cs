using Application.DTOs;
using UserEntity = User.Domain.Entities.User;

namespace Application.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task CreateAsync(UserDto user);
    Task UpdateAsync(UserDto user);
    Task DeleteAsync(string id);
    Task<UserDto> GetByEmailAsync(string email);
    Task<UserDto> GetByUsernameAsync(string username);
    Task<UserDto?> UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
    
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto);

    Task<List<MentorMatchDto>?> GetMentorsBySkillsWithScoreAsync(List<string> skills);
    
    Task SetMentorAvailabilityAsync(string mentorId, List<AvailabilityDto> availabilityDtos);
    Task<List<UserDto>> SearchMentorsBySkillAndAvailabilityAsync(string skill, DateTime desiredDateTime);
    
    Task<bool> UpdateMentorSkillsAsync(string userId, List<string> skills);


}