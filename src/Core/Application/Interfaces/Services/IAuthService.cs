using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task CreateUserAsync(RegisterRequestDto registerDto);
    Task<UserDto?> ValidateUserAsync(string loginDtoUsername, string loginDtoPassword);
    
    Task<AuthResponseDto?> LoginAsync(string input, string password);
    
    string GenerateJwtToken(UserDto user);

    string GenerateRefreshToken();
    Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string userId);
}