using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using User.Domain.Entities;
using User.Domain.Repositories;
using UserEntity = User.Domain.Entities.User;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository,IAuthRepository authRepository,IMapper mapper,IConfiguration configuration,ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _authRepository = authRepository;
        _mapper = mapper;
        _configuration = configuration;
        _logger = logger;
    }
    public async Task CreateUserAsync(RegisterRequestDto registerDto)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var newUser = new UserEntity()
        {
            Id = Guid.NewGuid().ToString(),
            Name = registerDto.Name,
            Username = registerDto.Username,
            Email = registerDto.Email,
            Password = passwordHash,
            Role = registerDto.Role,
            Skills = registerDto.Skills ?? new List<string>(),
        };
        
        await _userRepository.CreateAsync(newUser);
        _logger.LogInformation("User '{Username}' Created Succesfully", newUser.Username);

    }
    
    public async Task<UserDto?> ValidateUserAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null || string.IsNullOrWhiteSpace(user.Password))
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;
        _logger.LogInformation("User '{Username}' validation is done", user.Username);

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }
    
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        _logger.LogInformation("New Refresh token generated");

        return Convert.ToBase64String(randomBytes);
    }
    
     public async Task<AuthResponseDto?> LoginAsync(string input, string password)
    {
        UserDto? userDto = null;

        if (IsValidEmail(input))
        {
            var userByEmail = await _userRepository.GetByEmailAsync(input);
            if (userByEmail != null)
            {
                userDto = await ValidateUserAsync(userByEmail.Username, password);
            }
        }
        else
        {
            userDto = await ValidateUserAsync(input, password);
        }

        if (userDto == null) return null;

        var token = GenerateJwtToken(userDto);
        var refreshToken = GenerateRefreshToken();

        var userEntity = await _userRepository.GetByIdAsync(userDto.Id);
        userEntity.RefreshToken = refreshToken;
        userEntity.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateAsync(userEntity);
        _logger.LogInformation("User '{Username}' logged in Successfully", userDto.Username);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public string GenerateJwtToken(UserDto user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
            signingCredentials: creds
        );
        _logger.LogInformation("JWT Token generated for user '{Username}'", user.Username);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static bool IsValidEmail(string email) =>
        new EmailAddressAttribute().IsValid(email);

    // ✅ NEW: Refresh Token Flow
    public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var user = await _authRepository.GetByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return null;

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };

        var newAccessToken = GenerateJwtToken(userDto);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateAsync(user);

        return new AuthResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task LogoutAsync(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = DateTime.MinValue;
        _logger.LogInformation("User '{Username}' logged out successfully", user.Username);

        await _userRepository.UpdateAsync(user);
    }
}