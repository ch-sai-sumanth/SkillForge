using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task CreateUserAsync(RegisterRequestDto registerDto);
    Task<UserDto?> ValidateUserAsync(string loginDtoUsername, string loginDtoPassword);
}