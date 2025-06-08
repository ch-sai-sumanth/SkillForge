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
    Task CreateUserAsync(RegisterRequestDto registerDto);
    Task<UserDto?> ValidateUserAsync(string loginDtoUsername, string loginDtoPassword);
    Task<UserDto?> UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
}