using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using User.Domain.Entities;
using User.Domain.Repositories;
using UserEntity = User.Domain.Entities.User;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;


    public UserService(IUserRepository userRepository,IMapper mapper,ILogger<UserService> logger)
    {
        this._userRepository = userRepository;
        this._mapper = mapper;
        _logger = logger;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(user => _mapper.Map<UserDto>(user)).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task CreateAsync(UserDto userDto)
    {
        var user = _mapper.Map<UserEntity>(userDto);
        
        // Generate new ID for creation (ignore any ID from DTO)
        user.Id = Guid.NewGuid().ToString();
        await _userRepository.CreateAsync(user);
        _logger.LogInformation("User '{Username}' has been created.", user.Username);

    }

    public async Task UpdateAsync(UserDto userDto)
    {
        var user = _mapper.Map<UserEntity>(userDto);
        await _userRepository.UpdateAsync(user);
        _logger.LogInformation("User '{Username}' updated their details..", user.Username);

    }

    public async Task DeleteAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        await _userRepository.DeleteAsync(id);
        _logger.LogInformation("User '{Username}' is deleted", user.Username);

    }


    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }


    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user is null ? null : _mapper.Map<UserDto>(user);

    }



    public async Task<UserDto?> UpdateProfileAsync(string userId, UpdateUserProfileDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return null;

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Username = dto.Username;
        user.Skills = dto.Skills;

        await _userRepository.UpdateAsync(user);
        _logger.LogInformation("User '{Username}' Profile updated", user.Username);

        
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Username = user.Username,
            Skills = user.Skills
        };
    }
    
    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        // Verify old password
        var isMatch = BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password);
        if (!isMatch) return false;

        // Hash and set new password
        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);
        _logger.LogInformation("User '{Username}' Changed their Password", user.Username);


        return true;
    }

    
    public async Task<List<MentorMatchDto>> GetMentorsBySkillsWithScoreAsync(List<string> skills)
    {
        var mentors = await _userRepository.GetMentorsBySkillsAsync(skills);
    
        var mentorMatches = mentors.Select(mentor =>
            {
                var matchedSkillsCount = mentor.Skills.Intersect(skills, StringComparer.OrdinalIgnoreCase).Count();
                return new MentorMatchDto
                {
                    Mentor = _mapper.Map<UserDto>(mentor),
                    MatchScore = matchedSkillsCount
                };
            })
            .OrderByDescending(m => m.MatchScore)
            .ToList();

        return mentorMatches;
    }
        
    public async Task SetMentorAvailabilityAsync(string mentorId, List<AvailabilityDto> availabilityDtos)
    {
        var mentor = await _userRepository.GetByIdAsync(mentorId);
        if (mentor == null) throw new Exception("Mentor not found");

        mentor.Availabilities = availabilityDtos.Select(a => new Availability
        {
            Day = a.Day,
            StartTime = a.StartTime,
            EndTime = a.EndTime
        }).ToList();

        await _userRepository.UpdateAsync(mentor);
    }
    
    public async Task<List<UserDto>> SearchMentorsBySkillAndAvailabilityAsync(string skill, DayOfWeek day, TimeSpan time)
    {
        var mentors = await _userRepository.GetMentorsBySkillAsync(skill);

        var availableMentors = mentors.Where(m => m.Availabilities.Any(a =>
            a.Day == day &&
            a.StartTime <= time &&
            a.EndTime >= time)).ToList();

        return availableMentors.Select(m => _mapper.Map<UserDto>(m)).ToList();
    }
    

    public async Task<bool> UpdateMentorSkillsAsync(string userId, List<string> skills)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        user.Skills = skills;
        await _userRepository.UpdateAsync(user);

        return true;
    }

}